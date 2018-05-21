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
        private const string _serverRoot = "https://localhost:44300/api";

        private string _dokkaServerToken = null;
        public string DokkaServerToken { get { return _dokkaServerToken; } }

        private string _fileIdentifier = null;
        public string FileIdentifier { get { return _fileIdentifier; } }

        private ServerInterface()
        {
            
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

        public async Task UploadFileAtPath(string filePath, string fileName, Action<string> showLoginFunc, Action<string>uploadResultHandler)
        {
            if (String.IsNullOrEmpty(_dokkaServerToken))
            {
                Trace.WriteLine("User must login first");
                showLoginFunc("Login required to upload file to Dokka");

                return;
            }

            HttpClient httpClient = new HttpClient();
            // Auth header with token sent by the server
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _dokkaServerToken);

            MultipartFormDataContent form = new MultipartFormDataContent();
            var fileBytes = File.ReadAllBytes(filePath);
            form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "office_file", fileName);
            HttpResponseMessage response = await httpClient.PostAsync($"{_serverRoot}/values", form);
            var content = await response.Content.ReadAsStringAsync(); // check in the future if we want to implement a callback to the addin once upload is done / failed
            uploadResultHandler(content);
        }

        public async Task LoginWithCredentials(string username, string password, Action<bool> loginCallback)
        {
            HttpClient httpClient = new HttpClient();
            Dictionary<string, string> loginDic = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };
            var loginDataStr = JsonConvert.SerializeObject(loginDic);
            // StringContent postContent = new StringContent(loginDataStr);
            FormUrlEncodedContent postContent = new FormUrlEncodedContent(loginDic);
            HttpResponseMessage response = null;

            try
            {
                response = await httpClient.PostAsync($"{_serverRoot}/Login", postContent);
            } catch (Exception ex)
            {
                Trace.WriteLine($"Error when doing login: {ex.Message}\n{ex.InnerException?.Message ?? ""}");
                loginCallback(false);

                return;
            }

            if (response == null)
            {
                Trace.WriteLine("Login failure. Response is null. Super weird");
                loginCallback(false);

                return;
            } else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Trace.WriteLine($"Login Failure. Got Error {response.StatusCode} from server");
                loginCallback(false);

                return;
            }

            var content = response.Content;
            var contentStr = await content.ReadAsStringAsync();
            var result = (Dictionary<string, string>)JsonConvert.DeserializeObject(contentStr, typeof(Dictionary<string, string>));
            bool loginSuccess = false;
            string loginSuccessStr = $"Parsing Failure. Respose from server: {contentStr}";
            string filenameToken = null;
            string loginToken = null;

            try
            {
                loginSuccessStr = result["success"];
                loginToken = result["login-token"];
                filenameToken = result["file-token"];
                if (bool.TryParse(loginSuccessStr, out loginSuccess))
                {
                    Trace.WriteLine(result["details"]);
                }
                else
                {
                    Trace.WriteLine($"Failure parsing server respose, success str: {loginSuccessStr}");
                }
            } catch { }

            if (loginSuccess && (filenameToken != null))
            {
                _dokkaServerToken = loginToken;
                _fileIdentifier = filenameToken;
            } else
            {
                Trace.WriteLine($"Login failed: {result["details"]}");
            }

            loginCallback(loginSuccess);
        }
    }
}
