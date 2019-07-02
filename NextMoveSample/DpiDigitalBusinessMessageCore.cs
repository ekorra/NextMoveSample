using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NextMove.Lib
{
    public abstract class DpiDigitalBusinessMessageCore: BusinessMessageCore
    {
        [XmlElement("tittel")]
        public string Title { get; set; }

        [XmlElement("spraak")]
        public string Language { get; set; }

        [XmlElement("digitalPostInfo")]
        public DigitalPostInfo DigitalPostInfo { get; set; }


    }

    public class DigitalPostInfo
    {
        [XmlElement("varsler")]
        public Notification Notification { get; set; }

        [XmlIgnore]
        public DateTime EffectiveDateTime { get ; set; }

        [XmlElement("virkningsdato")]
        public String EffectiveDate
        {
            get { return EffectiveDateTime.Date.ToShortDateString(); }
        }
    }
}
