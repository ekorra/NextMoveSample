using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NextMove.Lib
{
    public abstract class DpiDigitalBusinessMessageCore: BusinessMessageCore
    {
        [XmlElement("ikkeSensitivTittel")]
        public string Title { get; set; }

        [XmlElement("spraak")]
        public string Language { get; set; }

        [XmlElement("varsler")]
        public Notification Notification { get; set; }

        [XmlElement("virkningsdato")]
        public DateTime EffectiveDate { get; set; }
    }
}
