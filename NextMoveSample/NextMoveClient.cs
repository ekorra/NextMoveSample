using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

            Debug.Write(sbd.ToJson2());

            content.Add(new StringContent(sbd.ToJson2().ToLower()), "sbd");
            content.Add(new StreamContent(File.OpenRead(filename)), "Test", "Test.jpg");
            

            var result = await httpClient.PostAsync("/api/messages/out/multipart", content);
            result.EnsureSuccessStatusCode();

            return "";
        }

        public async Task<string> SendLargeMessage(StandardBusinessDocument sbd, string contentPath)
        {
            var result = await httpClient.PostAsync("/api/messages/out", new StringContent(sbd.ToJson2()));
            //var conversationId = sbd.StandardBusinessDocumentHeader.DocumentIdentification.

            return "";
        }
    }

   
}
