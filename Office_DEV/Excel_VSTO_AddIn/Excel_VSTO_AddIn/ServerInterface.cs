using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Excel_VSTO_AddIn
{
    public class ServerInterface
    {
        private static ServerInterface _instance;
        //private const string _serverRoot = "https://localhost:44300/api";//http://api-dev.dokka.biz/api/v2/3/loginUser
        private const string CLIENT_PATH_PARAM = "3"; // 1 - iOS, 2 - Android, 3 - Web
        private string _serverRoot = $"http://api-dev.dokka.biz/api/v2/{CLIENT_PATH_PARAM}";
        public string DokkaServerToken { get; private set; } = null;

        private ServerInterface()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.loginToken))
            {
                DokkaServerToken = DokkaOfficeAddinConfiguration.Instance.LoginToken;
            }
        }

        public static ServerInterface Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServerInterface();
                }
                return _instance;
            }
        }

        public async Task UploadFileAtPath(string filePath, string fileName, Action<string> showLoginFunc, Action<string, bool>uploadResultHandler)
        {
            if (String.IsNullOrEmpty(DokkaServerToken))
            {
                Trace.WriteLine("User must login first");
                showLoginFunc("Login required to upload a file to Dokka");

                return;
            }

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
            httpClient.DefaultRequestHeaders.ExpectContinue = false;

            MultipartFormDataContent form = new MultipartFormDataContent(/*"DOKKA_EXCEL_UPLOADER_BOUNDRY"*/);
            var fileBytes = File.ReadAllBytes(filePath);
            // GZip is not yet supported by the server: var byteArrayContent = GCompress(fileBytes);
            var byteArrayContent = new ByteArrayContent(fileBytes);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = $"\"{fileName}\"", Name = "\"file\"" };
            // Uncomment if contentype is required (currently works without it and think about other formats such as CSV ...):
            // byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            HttpResponseMessage response = null;
            form.Add(byteArrayContent, "file", fileName);
            try
            {
                response = await httpClient.PostAsync($"{_serverRoot}/uploadDocument?dokkaToken={DokkaServerToken}", form);
            } catch (Exception e)
            {
                Trace.WriteLine($"Failure when attempting to upload file: {e.Message}\n{e.InnerException?.Message ?? ""}");
                uploadResultHandler("Upload failure. Cannot reach Dokka cloud.", false);

                return;
            }


            if (response == null)
            {
                Trace.WriteLine("Response is null. CHECK, this should not happen");
                uploadResultHandler("Upload failure. Internal Dokka problem", false);

                return;
            }
            else if ((response.StatusCode == System.Net.HttpStatusCode.OK) ||
              (response.StatusCode == System.Net.HttpStatusCode.NoContent))
            {
                /*
                 * Arik on Slack - 27 May 2018
                 * 204 - NoContent:
                 * 
                 * It’s a valid response
                 * 204 means  no response content
                 * 
                 * */
                Trace.WriteLine($"File {fileName} was successfully uloaded");
            } else
            {
                Trace.WriteLine($"There was an error when uploading file. CODE: {response.StatusCode}");
                bool showLogin = (response.StatusCode == System.Net.HttpStatusCode.Unauthorized);

                uploadResultHandler($"Upload to Dokka error. Error:{response.StatusCode}", showLogin);

                return;
            }

            var content = await response.Content.ReadAsStringAsync(); // check in the future if we want to implement a callback to the addin once upload is done / failed
            uploadResultHandler(String.IsNullOrEmpty(content) ? "Upload to Dokka Done" : content, false);
        }

        public async Task LoginWithCredentials(string userIdentifier, string password, string companyIdentifier, Action<bool, string> loginCallback)
        {
            HttpClient httpClient = new HttpClient();
            Dictionary<string, string> loginDic = new Dictionary<string, string>
            {
                { "email", userIdentifier },
                { "password", password },
                { "language", "en"}
            };

            var loginDataStr = JsonConvert.SerializeObject(loginDic);
            StringContent postContent = new StringContent(loginDataStr, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            // Login - Step 1
            try
            {
                response = await httpClient.PostAsync($"{_serverRoot}/loginUser", postContent);
            } catch (Exception ex)
            {
                Trace.WriteLine($"Error when doing login: {ex.Message}\n{ex.InnerException?.Message ?? ""}");
                loginCallback(false, null);

                return;
            }

            if (!ValidateResponse(response, out string responseProblem))
            {
                Trace.WriteLine($"Failure at login step 1: {responseProblem}");
                string loginErrorMessage = null;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    loginErrorMessage = "Invalid Email or Password";
                }
                loginCallback(false, loginErrorMessage);

                return;
            }

            var content = response.Content;
            var contentStr = await content.ReadAsStringAsync();

            string loginToken = null;

            try
            {
                var result = (Dictionary<string, string>)JsonConvert.DeserializeObject(contentStr, typeof(Dictionary<string, string>));
                loginToken = result["dokkaToken"];
            } catch(Exception ex)
            {
                Trace.WriteLine($"Failure extating token from response {contentStr}. Error: {ex.Message}\n{ex.InnerException?.Message ?? ""}");
            }
            
            if (String.IsNullOrEmpty(loginToken))
            {
                Trace.WriteLine($"Login failed: The login token wasn't supplied by the server.");

                loginCallback(false, null);
                return;
            }


            // Login - Step Two
            loginDic = new Dictionary<string, string>
            {
                { "email", companyIdentifier },
                { "t1Token", loginToken },
                { "language", "en"}
            };
            loginDataStr = JsonConvert.SerializeObject(loginDic);
            postContent = new StringContent(loginDataStr, Encoding.UTF8, "application/json");
            try
            {
                response = await httpClient.PostAsync($"{_serverRoot}/loginUser", postContent);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error when doing login: {ex.Message}\n{ex.InnerException?.Message ?? ""}");
                loginCallback(false, null);

                return;
            }
            if (!ValidateResponse(response, out string problemsStr))
            {
                Trace.WriteLine($"Failure at login step 2: {problemsStr}");
                loginCallback(false, null);

                return;
            }
            content = response.Content;
            contentStr = await content.ReadAsStringAsync();
            try
            {
                var result = (Dictionary<string, string>)JsonConvert.DeserializeObject(contentStr, typeof(Dictionary<string, string>));
                loginToken = result["dokkaToken"];
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Login Step2 failure extating token from response {contentStr}. Error: {ex.Message}\n{ex.InnerException?.Message ?? ""}");
                loginToken = null;
            }
            
            if (String.IsNullOrEmpty(loginToken))
            {
                Trace.WriteLine($"Login Step 2 failed: The login token wasn't supplied by the server.");
                
                loginCallback(false, null);
                return;
            }
            else
            {
                DokkaServerToken = loginToken;
                DokkaOfficeAddinConfiguration.Instance.LoginToken = DokkaServerToken;
            }

            loginCallback(true, loginToken);
        }

        public void ResetLoginToken()
        {
            DokkaServerToken = null;
        }

        private bool ValidateResponse(HttpResponseMessage response, out string errorMessage)
        {
            if (response == null)
            {
                errorMessage = "Login failure. Response is null. Super weird";

                return false;
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorMessage = $"Login Failure. Got Error {response.StatusCode} from server. User is not valid";
                } else
                {
                    errorMessage = $"Login Failure. Got Error {response.StatusCode} from server";
                }
                

                return false;
            }

            errorMessage = null;
            return true;
        }

        private static ByteArrayContent GCompress(byte[] Bytes)
        {
            // Compress given data using gzip 
            using (var Stream = new MemoryStream())
            {
                using (var Zipper = new GZipStream(Stream, CompressionMode.Compress, true)) Zipper.Write(Bytes, 0, Bytes.Length);
                var Content = new ByteArrayContent(Stream.ToArray());
                Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Content.Headers.ContentEncoding.Add("gzip");
                return Content;
            }
        }
    }
}
