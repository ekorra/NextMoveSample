using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonCustomSerializer()); 
        }

        public static StandardBusinessDocument ParseJson(string json)
        {
            return JsonConvert.DeserializeObject<StandardBusinessDocument>(json);
        }

        public static StandardBusinessDocument ParseXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(StandardBusinessDocument));
            using (TextReader reader = new StringReader(xml))
            {
                return (StandardBusinessDocument) serializer.Deserialize(reader);
            }
        }

        private static XmlElement SerializeToXmlElement(object o)
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
            List<Scope> s = new List<Scope>();
            var scope = new Scope();

            s.Add(new Scope());

            var sbdh = new StandardBusinessDocumentHeader
            {
                HeaderVersion = "1.0",
                Sender = new[] {GetPartner( envelopeInfo.SenderOrganisationNumber.ToString())},
                Receiver = new[] {GetPartner(envelopeInfo.ReceiverOrganisationNumber.ToString())},
                DocumentIdentification = GetDocumentIdentification(envelopeInfo),
                BusinessScope = new BusinessScope { Scope = GetBusniessScopes(envelopeInfo)}
            };

           

            return sbdh;
        }

        private Partner GetPartner(string id)
        {
            var p = new Partner[1];
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
            var scopes = new List<Scope>();

            scopes.Add(new Scope
            {
                Type = "ConversationId",
                InstanceIdentifier = envelopeInfo.ConversationId.ToString(),
                Identifier = envelopeInfo.ProcessId,
                ScopeInformation = new[]
                    {new CorrelationInformation {ExpectedResponseDateTime = DateTime.Now.AddHours(1)}}
            });

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
//}
}