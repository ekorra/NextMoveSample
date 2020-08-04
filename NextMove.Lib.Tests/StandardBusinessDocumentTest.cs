using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NextMove.Lib.Tests
{
    public class StandardBusinessDocumentTest
    {
        public class StandardBusinessDocument_ToJsonShold
        {
            private string validOrgNr = "910076787";
            private string validProcessId = "urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0";
            private string validDocumentId = "urn:no:difi:arkivmelding:xsd::arkivmelding";

            [Fact]
            public void ContainsAvtaltMeldingJson()
            {
                EnvelopeInfo envelopeInfo = new EnvelopeInfo(validOrgNr, 
                    validOrgNr, validProcessId, validDocumentId);


                var businessMessage = GetDpaBusinesmessage(string.Empty);

                StandardBusinessDocument sbd = new StandardBusinessDocument(envelopeInfo, businessMessage);

                var result = sbd.ToJson();
                var jObject = JObject.Parse(sbd.ToJson());
                var avtalt = jObject["avtalt"];


                Assert.True(avtalt.HasValues);
            }
            
            [Fact]
            public void ContainsContenttMeldingJson()
            {
                EnvelopeInfo envelopeInfo = new EnvelopeInfo(validOrgNr, 
                    validOrgNr, validProcessId, validDocumentId);

                var contentValue = GetContent();
                var businessMessage = GetDpaBusinesmessage(contentValue);

                StandardBusinessDocument sbd = new StandardBusinessDocument(envelopeInfo, businessMessage);

                var result = sbd.ToJson();
                var jObject = JObject.Parse(sbd.ToJson());
                var content = JObject.Parse( (string)jObject["avtalt"]["content"]);
                

                Assert.True(content.HasValues);
            }

            private DpaBusinessMessage GetDpaBusinesmessage(string content)
            {
                var businessMessage = new DpaBusinessMessage();
                businessMessage.SecurityLevel = 4;
                businessMessage.PrimaryDocumentName = "somedocument.pdf";
                businessMessage.identifier = "no.difi.avtalt.test.v1";

               

                if(!string.IsNullOrEmpty(content))
                {
                    businessMessage.content = content;
                }

                return businessMessage;
            }

            private string GetContent()
            {
                var sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    using (var writer = new JsonTextWriter(sw))
                    {
                        writer.Formatting = Formatting.None;

                        writer.WriteStartObject();
                        writer.WritePropertyName(@"eksempel");
                        writer.WriteValue(@"verdi");
                        writer.WritePropertyName("eksempelObjekt");
                        writer.WriteStartObject();
                        writer.WritePropertyName("eksempel2");
                        writer.WriteValue("verdi2");
                        writer.WritePropertyName("eksempel3");
                        writer.WriteValue("verdi3");
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                }

                return sb.ToString();
            }
        }
    }
}
