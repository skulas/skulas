using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using System.Diagnostics;
using System.Web;
using System.IO;

namespace office_uploads.Controllers
{
    // Override the Get Local Filename method, so file is concistent with uploaded file.
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path)  { }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"",string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        }
    }

    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return $"value: {id}";
        }

        // POST api/values
        //public void Post([FromBody]string value)
        //{
        //    string strb = $"You sent me {value}. You can keep it";
        //}
        public async Task<HttpResponseMessage> Put()
        //public async Task<HttpResponseMessage> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/uploads");
            var provider = new CustomMultipartFormDataStreamProvider(root);
            var props = Request.Properties;
            var rqStr = Request.ToString();
            Trace.WriteLine("Request:");
            Trace.WriteLine(rqStr);
            Trace.WriteLine("======================");

            var headers = Request.Headers;
            Trace.WriteLine("headers");
            foreach (var header in headers)
            {
                Trace.WriteLine($"x.x.x.x.x {header.Key} x.x.x.x.x");
                IEnumerable<string> values = null;
                if (headers.TryGetValues(header.Key, out values))
                {
                    foreach (var value in values)
                    {
                        Trace.WriteLine($"{value}");
                    }
                }
            }

            try
            {
                var tito = await Request.Content.ReadAsByteArrayAsync();
                Trace.WriteLine($"Received {tito.Length} bytes");

                return Request.CreateResponse(HttpStatusCode.OK);
            } catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            

            //try
            //{
            //    // Read the form data.
            //    await Request.Content.ReadAsMultipartAsync(provider);

            //    // This illustrates how to get the file names.
            //    foreach (MultipartFileData file in provider.FileData)
            //    {
            //        Trace.WriteLine(file.Headers.ContentDisposition.FileName);
            //        Trace.WriteLine("Server file path: " + file.LocalFileName);
            //    }
            //    return Request.CreateResponse(HttpStatusCode.OK);
            //}
            //catch (System.Exception e)
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            //}




            /////////////////////
            //HttpRequestMessage request = this.Request;
            //if (!request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads");
            //var provider = new MultipartFormDataStreamProvider(root);

            //var task = request.Content.ReadAsMultipartAsync(provider).
            //    ContinueWith<HttpResponseMessage>(o =>
            //    {

            //        //string file1 = provider.BodyPartFileNames.First().Value;
            //        var firstFile = provider.FileData.FirstOrDefault();
            //        string filename = "";
            //        if (firstFile != null)
            //        {
            //            filename = firstFile.Headers.ContentDisposition.FileName;
            //        }
            //        // this is the file name on the server where the file was saved 

            //        return new HttpResponseMessage()
            //        {
            //            Content = new StringContent($"File uploaded: {filename}")
            //        };
            //    }
            //);
            //return task;
        }

        // PUT api/values/5
//        public void Put(int id, [FromBody]string value)
        public async Task<HttpResponseMessage> Post()
        {
            string root = HttpContext.Current.Server.MapPath("~/App_Data/uploads");
            var props = Request.Properties;
            var rqStr = Request.ToString();
            Trace.WriteLine("Request:");
            Trace.WriteLine(rqStr);
            Trace.WriteLine("======================");

            var headers = Request.Headers;
            Trace.WriteLine("headers");
            foreach (var header in headers)
            {
                Trace.WriteLine($"x.x.x.x.x {header.Key} x.x.x.x.x");
                IEnumerable<string> values = null;
                if (headers.TryGetValues(header.Key, out values))
                {
                    foreach (var value in values)
                    {
                        Trace.WriteLine($"{value}");
                    }
                }
            }

            try
            {
                var tito = await Request.Content.ReadAsByteArrayAsync();
                Trace.WriteLine($"Received {tito.Length} bytes");
                File.WriteAllBytes(@"D:\DEV\Dokka\Office_addon\skulas\Office_DEV\office_uploads_server\office_uploads\App_Data\uploads\cabron.xlsx", tito);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
