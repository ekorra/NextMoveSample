using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using NextMove.Lib;

namespace NextMoveSample.console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var digitalSbd = GetDpiDigitalMessageSbd("0192:910076787", "06068700602");
            //var json = digitalSbd.ToJson2();
            //var xml = digitalSbd.ToXml();
            //await SaveToFile(json, @"C:\temp\nextmove\digital.json");
            //await SaveToFile(xml, @"C:\temp\nextmove\digital.xml");
            //var json = System.IO.File.ReadAllText(@"c:\temp\jsonNoCase.json");
            //var parsedJson = StandardBusinessDocument.ParseJson(json);
            //var json1 = System.IO.File.ReadAllText(@"c:\temp\jsonNoCaseNoAny.json");
            //var parsedJson1 = StandardBusinessDocument.ParseJson(json);
            //var json2 = System.IO.File.ReadAllText(@"c:\temp\jsonNoCaseNoNamespace.json");
            //var parsedJson2 = StandardBusinessDocument.ParseJson(json);

            //var xml = digitalSbd.ToXml();
            //var parsedXml = StandardBusinessDocument.ParseXml(xml);

            var httpClient  = new HttpClient();
            var nextMoveClient = new NextMoveClient(httpClient);
            //string result;

            //if (!await nextMoveClient.IsIpRunning())
            //{
            //    return;
            //}

            try
            {
                var d = digitalSbd.ToJson();
                var result = await nextMoveClient.SendSmallMessage(digitalSbd, @"C:\temp\nextmove\Test.pdf");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        
        

        private static async Task SaveToFile(string content, string filename)
        {
            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                await outputFile.WriteAsync(content);
            }
        }

        private static StandardBusinessDocument GetDpiDigitalDpvMessage(string senderId, string receiverId)
        {
            var dpiDigitalDpvBusinessMessage = new DpiDigitalDpvBusinessMessage
            {
                Title = "test tittel",
                Body = "Body",
                Summary = "Summary",
                Language = "NO",
                SecurityLevel = 3,
                PrimaryDocumentName = "Test.pdf",
                DigitalPostInfo = new DigitalPostInfo { EffectiveDateTime = DateTime.Now}
            };

            return new StandardBusinessDocument(new EnvelopeInfo(senderId, receiverId, "urn:no:difi:profile:digitalpost:info:ver1.0", "urn:no:difi:digitalpost:xsd:digital::digital_dpv"), dpiDigitalDpvBusinessMessage);
        }

        private static StandardBusinessDocument GetDpiPrintMessage(string senderId, string receiverId)
        {
            var receiver = new Receiver
            {
                Name = "Test Testeren",
                AddressLine1 = "Adresselinje1",
                AddressLine2 = "Adresselinje2",
                AddressLine3 = "Adresselinje3",
                AddressLine4 = "Adresselinje4",
                ZipCode = "0000",
                City = "Poststed",
                Country = "Norge",
                CountryCode = "NO"
            };

            var dpiPrintBusinessMessage = new DpiPrintBusinessMessage
            {
                SecurityLevel = 3,
                PostalType = "B",
                PrimaryDocumentName = "Test.pdf",
                PrintColor = "SORT_HVITT",
                Receiver = receiver,
                ReturnPost = new ReturnPost
                {
                    Receiver = receiver, PostHandling = "DIREKTE_RETUR"
                }
            };

            var sbd = new StandardBusinessDocument(
                new EnvelopeInfo(senderId, receiverId, "urn:no:difi:profile:digitalpost:vedtak:ver1.0",
                    "urn:no:difi:digitalpost:xsd:fysisk::print"), dpiPrintBusinessMessage);
            return sbd;
        }

        private static StandardBusinessDocument GetDpiDigitalMessageSbd(string senderId, string receiverId)
        {
            var dpiDigitalMessage = new DpiDigitalBusinessMessage
            {
                
                Language = "NO",
                Title = "tittel",
                
                SecurityLevel = 3,
                PrimaryDocumentName = "test.pdf",
                DigitalPostInfo = new DigitalPostInfo
                {
                    ReceiptOnOpening = false,
                    EffectiveDateTime = DateTime.Now.AddDays(1),
                },
                Notification = new Notification
                {
                    EmailText = "ePost varsel",
                    SmsText = "SMS varsel"

                }

            };

            var envelope = new EnvelopeInfo(senderId, receiverId, "urn:no:difi:profile:digitalpost:info:ver1.0",
                "urn:no:difi:digitalpost:xsd:digital::digital");
            //var sbd = new StandardBusinessDocument();
           // sbd.StandardBusinessDocumentHeader = 

            var sbd = new StandardBusinessDocument(envelope, dpiDigitalMessage);
            return sbd;
        }
    }
}
