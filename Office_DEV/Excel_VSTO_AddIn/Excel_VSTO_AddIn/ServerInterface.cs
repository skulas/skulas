using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Excel_VSTO_AddIn
{
    public class ServerInterface
    {
        private static ServerInterface _instance;
        //private const string _serverRoot = "https://localhost:44300/api";//http://api-dev.dokka.biz/api/v2/3/loginUser
        private const string _serverRoot = "http://api-dev.dokka.biz/api/v2/3";

        private string _dokkaServerToken = null;
        public string DokkaServerToken { get { return _dokkaServerToken; } }

        private ServerInterface()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.loginToken))
            {
                _dokkaServerToken = Properties.Settings.Default.loginToken;
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
            if (String.IsNullOrEmpty(_dokkaServerToken))
            {
                Trace.WriteLine("User must login first");
                showLoginFunc("Login required to upload a file to Dokka");

                return;
            }

            HttpClient httpClient = new HttpClient();
            // Auth header with token sent by the server
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _dokkaServerToken);

            MultipartFormDataContent form = new MultipartFormDataContent();
            var fileBytes = File.ReadAllBytes(filePath);
            form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "office_file", fileName);
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.PostAsync($"{_serverRoot}/values", form);
            } catch (Exception e)
            {
                Trace.WriteLine($"Failure when attempting to upload file: {e.Message}\n{e.InnerException?.Message ?? ""}");
                uploadResultHandler("Upload crashed when calling the server", false);

                return;
            }


            if (response == null)
            {
                Trace.WriteLine("Response is null. CHECK, this should not happen");
                uploadResultHandler(null, false);

                return;
            } else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Trace.WriteLine($"There was an error when uploading file. CODE: {response.StatusCode}");
                bool showLogin = (response.StatusCode == System.Net.HttpStatusCode.Unauthorized);

                uploadResultHandler(null, showLogin);

                return;
            }

            var content = await response.Content.ReadAsStringAsync(); // check in the future if we want to implement a callback to the addin once upload is done / failed
            uploadResultHandler(content, false);
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
            // StringContent postContent = new StringContent(loginDataStr);
            FormUrlEncodedContent postContent = new FormUrlEncodedContent(loginDic);
            HttpResponseMessage response = null;

            // First Login
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
                    loginErrorMessage = "Invalid Username or Password";
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


            // Login Step Two
            loginDic = new Dictionary<string, string>
            {
                { "email", companyIdentifier },
                { "t1Token", loginToken },
                { "language", "en"}
            };
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
                _dokkaServerToken = loginToken;
                Properties.Settings.Default.loginToken = _dokkaServerToken;
                Properties.Settings.Default.Save();
            }

            loginCallback(true, loginToken);
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
    }
}
