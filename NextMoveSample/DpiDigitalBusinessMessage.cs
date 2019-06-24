using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace NextMove.Lib
{
    [XmlRoot("digital")]
    public class DpiDigitalBusinessMessage: DpiDigitalBusinessMessageCore
    {

        [XmlElement("aapningskvittering")]
        public bool ReceiptOnOpening { get; set; }

    }
}
