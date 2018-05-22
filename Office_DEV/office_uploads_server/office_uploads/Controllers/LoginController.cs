using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace office_uploads.Controllers
{
    public class LoginController : ApiController
    {
        public static string DummyTokenForTesting { get; set; } = "FFF000FFFB";

        // GET: api/Login
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Login/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Login
        public async Task<HttpResponseMessage> Post(/*[FromBody]string value*/)
        {
            var requestStr = Request.Content.ToString();
            // Dictionary<string, string> requestDic;
            var requestContent = await Request.Content.ReadAsFormDataAsync();
            var username = requestContent["username"];
            var pwd = requestContent["password"];
            bool loginResult = false;
            string loginMessage = "Bad Login. Use u:aaa  p:zzz to login";
            if (username.Equals("aaa") && pwd.Equals("zzz"))
            {
                loginResult = true;
                loginMessage = "Great Success";
            }
            HttpStatusCode httpCode = loginResult ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;


            var response = Request.CreateResponse(httpCode, loginMessage);

            Dictionary<string, string> resultDic = new Dictionary<string, string>()
            {
                {"success", loginResult.ToString() },
                {"details", loginMessage },
                {"login-token", (loginResult ? DummyTokenForTesting : "") },
                {"file-token", "bad1234dad" }
            };
            
            response.Content = new StringContent(JsonConvert.SerializeObject(resultDic));

            //var responseTask = new Task(() =>
            //{
            //    return response;
            //});
            return response;
        }

        // PUT: api/Login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }
}
