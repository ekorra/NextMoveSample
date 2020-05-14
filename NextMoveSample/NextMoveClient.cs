using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HeyRed.Mime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NextMove.Lib
{
    public class NextMoveClient : INextMoveClient
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
                Debug.WriteLine("-------------Request-------------");
                Debug.WriteLine(await httpResponseMessage.RequestMessage.Content.ReadAsStringAsync());

                Debug.WriteLine("---------------------------------");
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
            if (string.IsNullOrEmpty(orgnr)) { throw new ArgumentException(nameof(orgnr)); }
            var result = await httpClient.GetAsync($"/api/capabilities/{orgnr}");
            if (!result.IsSuccessStatusCode) throw new Exception($"Failed getting capabilities:  {result.StatusCode}");

            var capabilities = Capabilities.FromJson(await result.Content.ReadAsStringAsync());

            return capabilities;
        }

        public async Task<IList<StandardBusinessDocument>> GetAllMessages()
        {

            var httpResponseMessage = await httpClient.GetAsync("/api/messages/in");
            if (!httpResponseMessage.IsSuccessStatusCode) { throw new Exception("Henting feilet");}

            var messages = JsonConvert.DeserializeObject<Rootobject>(await httpResponseMessage.Content.ReadAsStringAsync());
            return messages.content;
        }

        public async Task<StandardBusinessDocument> GetMessage(MessageTypes messageType, DirectoryInfo storagePath)
        {
            var message = await PeekMessage(messageType);
            if (message == null) { return null; }

            var messageId = message.StandardBusinessDocumentHeader.DocumentIdentification.InstanceIdentifier;
            var payload = await GetPayload(messageId);

            if (!storagePath.Exists)
            {
                storagePath.Create();
            }

            storagePath.CreateSubdirectory(messageId);


            try
            {
                using (var fileStream = File.Create($@"{storagePath}\{messageId}\asic.zip"))
                {
                    payload.Seek(0, SeekOrigin.Begin);
                    payload.CopyTo(fileStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            await DeleteMessage(messageId);

            return message;
        }

        public Task<string> GetStatus(string messageId)
        {
            throw new NotImplementedException();
        }

        private async Task<StandardBusinessDocument> PeekMessage(MessageTypes messageType)
        {
            var httpResponseMessage = await httpClient.GetAsync(messageType==MessageTypes.ALL?"/api/messages/in/peek": $"/api/messages/in/peek?serviceIdentifier={messageType.ToString()}");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Peek feilet");
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<StandardBusinessDocument>(await httpResponseMessage.Content
                    .ReadAsStringAsync());
        }

        private async Task<Stream> GetPayload(string messageId)
        {
            if(string.IsNullOrEmpty(messageId)) { throw new ArgumentException(nameof(messageId)) ;}

            var httpResponseMessage = await httpClient.GetAsync($"/api/messages/in/pop/{messageId}");
            if (!httpResponseMessage.IsSuccessStatusCode) { throw new Exception("Payload feilet");}

            return await httpResponseMessage.Content.ReadAsStreamAsync();
        }

        private async Task DeleteMessage(string messageId)
        {
            if (string.IsNullOrEmpty(messageId)) { throw new ArgumentException(nameof(messageId)); }

            var httpResponseMessage = await httpClient.DeleteAsync($"/api/messages/in/{messageId}");
            if(!httpResponseMessage.IsSuccessStatusCode) 
            {
                throw new Exception("Slette melding feilet");
            }
        }


    }

    public enum MessageTypes
    {
        DPO,
        DPI,
        DPV,
        DPE,
        DPF,
        DPA,
        ALL
    }


}
