using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace NextMove.Lib
{
    public partial class StandardBusinessDocument
    {

        public StandardBusinessDocument(EnvelopeInfo envelopeInfo, BusinessMessageCore businessMessageCore)
        {
            StandardBusinessDocumentHeader = GetStandardBusinessDocumentHeader(envelopeInfo);
            Any = SerializeToXmlElement(businessMessageCore);
        }

        public StandardBusinessDocument()
        {
            additionalData = new Dictionary<string, JToken>();
        }

        [JsonExtensionData]
        private IDictionary<string, JToken> additionalData;


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            //arkivmelding is not deserialized to the correct type correctly
            if(additionalData.ContainsKey("arkivmelding"))
            {
               
                Any = SerializeToXmlElement(additionalData["arkivmelding"].ToObject<DpoBusinessMessage>());
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonCustomSerializer()); 
        }

        public static StandardBusinessDocument ParseJson(string json)
        {
            return JsonConvert.DeserializeObject<StandardBusinessDocument>(json);
        }

        public string ToXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(StandardBusinessDocument));
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, this);
                    return stringWriter.ToString();
                }
            }
        }

        public static StandardBusinessDocument ParseXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(StandardBusinessDocument));
            using (TextReader reader = new StringReader(xml))
            {
                return (StandardBusinessDocument) serializer.Deserialize(reader);
            }
        }

        internal static XmlElement SerializeToXmlElement(object o)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlWriter = xmlDocument.CreateNavigator().AppendChild())
            {
                new XmlSerializer(o.GetType() ).Serialize(xmlWriter, o);
            }
            return xmlDocument.DocumentElement;
        }


        private StandardBusinessDocumentHeader GetStandardBusinessDocumentHeader(EnvelopeInfo envelopeInfo)
        {

            var standardBusinessDocumentHeader = new StandardBusinessDocumentHeader
            {
                HeaderVersion = "1.0",
                Sender = new[] {GetPartner( envelopeInfo.SenderOrganisationNumber)},
                Receiver = new[] {GetPartner(envelopeInfo.ReceiverOrganisationNumber)},
                DocumentIdentification = GetDocumentIdentification(envelopeInfo),
                BusinessScope = new BusinessScope { Scope = GetBusniessScopes(envelopeInfo)}
            };

            return standardBusinessDocumentHeader;
        }

        private Partner GetPartner(string id)
        {
            var partner = new Partner
            {
                Identifier = new PartnerIdentification
                {
                    Authority = "iso6523-actorid-upis",
                    Value =$"0192:{id}"
                }
            };
            return  partner;
        }

        private DocumentIdentification GetDocumentIdentification(EnvelopeInfo envelopeInfo)
        {
            var documentIdentification =  new DocumentIdentification
            {
                Standard = envelopeInfo.DocumenttypeId,
                CreationDateAndTime = DateTime.Now,
                TypeVersion = "2.0",
                Type = envelopeInfo.ForettningsmeldingType
            };
            if (!string.IsNullOrEmpty(envelopeInfo.MessageId))
            {
                documentIdentification.InstanceIdentifier = envelopeInfo.MessageId;
            }

            return documentIdentification;
        }

        private List<Scope> GetBusniessScopes(EnvelopeInfo envelopeInfo)
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Type = "ConversationId",
                    InstanceIdentifier = envelopeInfo.ConversationId,
                    Identifier = envelopeInfo.ProcessId,
                    ScopeInformation = new[]
                    {
                        new CorrelationInformation {ExpectedResponseDateTime = DateTime.Now.AddHours(1)}
                    }
                }
            };


            if (!string.IsNullOrEmpty(envelopeInfo.SenderRef))
            {
                scopes.Add(new Scope
                {
                    Type = "SenderRef",
                    InstanceIdentifier = envelopeInfo.SenderRef,
                    Identifier = envelopeInfo.SendignSystem
                });
            }

            if (!string.IsNullOrEmpty(envelopeInfo.ReceiverRef ))
            {
                scopes.Add(new Scope
                {
                    Type = "ReceiverRef",
                    InstanceIdentifier = envelopeInfo.ReceiverRef,
                    Identifier = envelopeInfo.ReceivinSystem
                });
            }

            return scopes;
        }
    }
}