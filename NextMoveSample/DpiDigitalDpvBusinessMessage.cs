using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NextMove.Lib
{
    [XmlRoot("digital_dpv")]
    public class DpiDigitalDpvBusinessMessage: DpiDigitalBusinessMessageCore
    {
        [XmlElement("sammendrag")]
        public string Summary { get; set; }

        [XmlElement("innhold")]
        public string Body { get; set; }
    }
}
