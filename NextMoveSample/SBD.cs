using System;
using System.Collections.Generic;
using System.IO;
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

        public StandardBusinessDocument(SbdAddressInfo sbdAddressInfo, BusinessMessageCore businessMessageCore)
        {
            StandardBusinessDocumentHeader = GetStandardBusinessDocumentHeader(sbdAddressInfo);
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
        public string ToJson2()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ToXml());
            return JsonConvert.SerializeXmlNode(xmlDoc);

        }

       
        
        public string ToJson()
        {

            return JsonConvert.SerializeObject(this,

                     Formatting.Indented,
                     new JsonCustomSerializer());


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


        private StandardBusinessDocumentHeader GetStandardBusinessDocumentHeader(SbdAddressInfo sbdAddressInfo)
        {
            List<Scope> s = new List<Scope>();
            var scope = new Scope();

            s.Add(new Scope());

            var sbdh = new StandardBusinessDocumentHeader
            {
                HeaderVersion = "1.0",
                Sender = new[] {GetPartner( sbdAddressInfo.SenderOrganisationNumber.ToString())},
                Receiver = new[] {GetPartner(sbdAddressInfo.ReceiverOrganisationNumber.ToString())},
                DocumentIdentification = GetDocumentIdentification(sbdAddressInfo),
                BusinessScope = GetBusniessScopes()
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
                    Value = $"0192:{id}"
                }
            };
            return  partner;
        }

        private DocumentIdentification GetDocumentIdentification(SbdAddressInfo sbdAddressInfo)
        {
            return new DocumentIdentification
            {
                Standard = sbdAddressInfo.DocumenttypeId,
                CreationDateAndTime = DateTime.Now,
                TypeVersion = "2.0",
                InstanceIdentifier = Guid.NewGuid().ToString(),
                Type = sbdAddressInfo.ForettningsmeldingType
            };
        }

        private Scope[] GetBusniessScopes()
        {
            
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Type = "ConversationId",
                    InstanceIdentifier = "6d0ec155-b4f7-43c3-908e-211d68c9cf09",
                    Identifier = "urn:no:difi:sdp:1.0",
                    ScopeInformation = new[]
                        {new CorrelationInformation {ExpectedResponseDateTime = DateTime.Now.AddHours(1)}}
                },
                new Scope
                {
                    Type = "SenderRef",
                    InstanceIdentifier = Guid.NewGuid().ToString(),
                    Identifier = "AvsenderSystem"
                },
                new Scope
                {
                    Type = "ReceiverRef",
                    InstanceIdentifier = Guid.NewGuid().ToString(),
                    Identifier = "MottakerSystem"
                }
            };


            return scopes.ToArray();
        }
    }
//}
}