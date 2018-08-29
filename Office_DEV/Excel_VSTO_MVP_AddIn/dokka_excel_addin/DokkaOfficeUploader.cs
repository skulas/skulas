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

namespace dokka_excel_addin
{
    public class DokkaOfficeUploader
    {

        #region Life Cycle

        public static DokkaOfficeUploader shared = new DokkaOfficeUploader();

        // Consts

        // Class HttpClient was planned to be reused, as the docs say.
        private HttpClient _httpClient = null;

        private DokkaOfficeUploader()
        {
            SetupHttpClient();
        }

        private void SetupHttpClient()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
            _httpClient.DefaultRequestHeaders.ExpectContinue = false;

        }

        #endregion Life Cycle


        #region Public

        public async Task UploadFileAtPath(string filePath, string fileName, Action<string, string> uploadResultHandler)
        {
            // Load file from HD
            var fileBytes = File.ReadAllBytes(filePath);

            // Turn file into bytearray
            // GZip is not yet supported by the server. Once it is, use this line
            // var byteArrayContent = GCompress(fileBytes);
            var byteArrayContent = new ByteArrayContent(fileBytes);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = $"\"{fileName}\"", Name = "\"file\"" };

            // Wrap bytearray in a POST request and send it.
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpResponseMessage response = null;
            try
            {
                form.Add(byteArrayContent, "file", fileName);
            } catch (Exception e)
            {
                Trace.TraceError($"Error when adding file to MultipartFormDataContent object: {e.Message}\n{e.InnerException?.Message ?? ""}");
                uploadResultHandler("Upload failure. Cannot encode the file into a Dokka request.", null);

                return;
            }
            
            try
            {
                response = await _httpClient.PostAsync($"{DokkaConfig.shared.ServerRoot}/uploadDocumentVersion", form);
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failure when attempting to upload file: {e.Message}\n{e.InnerException?.Message ?? ""}");
                uploadResultHandler("Upload failure. Cannot reach Dokka cloud.", null);

                return;
            }

            string dokkaDocId = null;
            string STUB_dokka_doc_id_STUB = $"temp_dokka_doc_id_{DateTime.Now.ToString("yyyy_MMM_dd__HH_mm__!@#$%^")}";
            if (response == null)
            {
                Trace.WriteLine("Response is null. CHECK, this should not happen");
                uploadResultHandler("Upload failure. Internal Dokka problem", null);

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
                Trace.TraceWarning("Using a locally generated Doc ID stub");
                dokkaDocId = STUB_dokka_doc_id_STUB;
            }
            else
            {
                Trace.WriteLine($"There was an error when uploading file. CODE: {response.StatusCode}");

                uploadResultHandler(null, null);

                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            uploadResultHandler(String.IsNullOrEmpty(content) ? "Upload to Dokka Done" : $"Upload to Dokka: {content}", dokkaDocId);
        }

        #endregion Public



        #region Utils

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

        #endregion Utils

    }
}
