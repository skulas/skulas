using System;
using System.Collections.Generic;
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

        public string DokkaServerToken { get; } = null;

        private ServerInterface()
        {
            DokkaServerToken = null;
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

        public async Task UploadFileAtPath(string filePath, string fileName)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            var fileBytes = File.ReadAllBytes(filePath);
            form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "office_file", fileName);
            HttpResponseMessage response = await httpClient.PostAsync($"{_serverRoot}/values", form);
            var content = response.Content; // check in the future if we want to implement a callback to the addin once upload is done / failed
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
            HttpResponseMessage response = await httpClient.PostAsync($"{_serverRoot}/Login", postContent);
            var content = response.Content;
            var contentStr = content.ToString();
            loginCallback(true);
        }
    }
}
