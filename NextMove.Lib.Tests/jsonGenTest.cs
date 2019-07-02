using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using QuickType;
using Xunit;

namespace NextMove.Lib.Tests
{
    public class jsonGenTest
    {
        [Fact]
        public void ReturnTypedBusninessMessage()
        {
            Welcome<DpiDigitalBusinessMessage> w = new Welcome<DpiDigitalBusinessMessage>();
            w.StandardBusinessDocumentHeader = new QuickType.StandardBusinessDocumentHeader
            {
                HeaderVersion = "1.0"
            };
            w.BusniessObject = GetMessage();
            w.SomeObject = GetMessage();
            var res = QuickType.Serialize.ToJson(w);
        }

        private DpiDigitalBusinessMessage GetMessage()
        {
            var dpiDigitalMessage = new DpiDigitalBusinessMessage
            {

                Language = "NO",
                Title = "tittel",
                ReceiptOnOpening = false,
                SecurityLevel = 3,
                PrimaryDocumentName = "test.pdf",
                DigitalPostInfo = new DigitalPostInfo
                {
                    EffectiveDateTime = DateTime.Now.AddDays(1),
                    Notification = new Notification
                    {
                        EmailText = "ePost varsel",
                        SmsText = "SMS varsel"

                    }
                }
            };
            return dpiDigitalMessage;
        }
    }
}
