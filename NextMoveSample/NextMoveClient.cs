using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HeyRed.Mime;
using Newtonsoft.Json.Linq;

namespace NextMove.Lib
{
    public class NextMoveClient
    {
        private readonly HttpClient httpClient;

        public NextMoveClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("http://localhost:9093");
        }

        public async Task<bool> IsIpRunning()
        {
            var httpResponseMessage = await httpClient.GetAsync("manage/health");

            if (!httpResponseMessage.IsSuccessStatusCode) return false;

            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var status = (string) json["status"];
            return String.Equals(status, "up", StringComparison.CurrentCultureIgnoreCase);

        }

        public async Task<string> SendSmallMessage(StandardBusinessDocument sbd, string filename)
        {
            var content = new MultipartFormDataContent();

            Debug.Write(sbd.ToJson());
            
            content.Add(new StringContent(sbd.ToJson()), "sbd");
            content.Add(new StreamContent(File.OpenRead(filename)), "test.pdf", "test.pdf");

            var result = await httpClient.PostAsync("/api/messages/out/multipart", content);
            result.EnsureSuccessStatusCode();

            return "";
        }


        public async Task<bool> SendMessage(EnvelopeInfo envelopeInfo, BusinessMessageCore businessMessage,  IEnumerable<FileInfo> files)
        {
            if (!string.IsNullOrEmpty(envelopeInfo.MessageId))
            {
                envelopeInfo.MessageId = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrEmpty(envelopeInfo.ConversationId))
            {
                envelopeInfo.ConversationId = Guid.NewGuid().ToString();

            }
            var sbd = new StandardBusinessDocument(envelopeInfo, businessMessage );
            
            var httpResponseMessage = await httpClient.PostAsync("/api/messages/out", new StringContent(sbd.ToJson(), Encoding.UTF8, mediaType: "application/json"));
            
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return false;
            }

            foreach (var file in files)
            {
                httpResponseMessage = null;
                var content = new StreamContent(file.OpenRead());
                content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypesMap.GetMimeType(file.Name));
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = Path.GetFileNameWithoutExtension(file.Name),
                    FileName = file.Name
                };
                httpResponseMessage = await httpClient.PutAsync($"/api/messages/out/{envelopeInfo.MessageId}", content);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return false;
                }
            }

            httpResponseMessage = null;
            httpResponseMessage = await httpClient.PostAsync($"/api/messages/out/{envelopeInfo.MessageId}", new StringContent(""));
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return false;
            }


            return true;
        }

        public async Task<Capabilities> GetCapabilities(string orgnr)
        {
            var result = await httpClient.GetAsync($"/api/capabilities/{orgnr}");
            if (!result.IsSuccessStatusCode) throw new Exception($"Failed getting capabilities:  {result.StatusCode}");

            var capabilities = Capabilities.FromJson(await result.Content.ReadAsStringAsync());

            return capabilities;
        }
    }

   
}
