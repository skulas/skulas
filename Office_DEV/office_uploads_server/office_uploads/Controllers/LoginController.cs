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
            var result = await Request.Content.ReadAsFormDataAsync();
            var response = Request.CreateResponse(HttpStatusCode.OK, "Great Success");
            response.Content = new StringContent("{ success:true }");

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
