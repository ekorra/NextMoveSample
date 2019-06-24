using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<string> SendSmallMessage(StandardBusinessDocument sbd, string filename)
        {
            var content = new MultipartFormDataContent
            {
                new StreamContent(File.OpenRead(filename)),
                new StringContent(sbd.ToJson2())
            };

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
