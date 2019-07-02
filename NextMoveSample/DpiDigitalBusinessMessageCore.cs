using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

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
        [XmlElement("aapningskvittering")]
        public bool ReceiptOnOpening { get; set; }

        //[XmlIgnore]
        [XmlElement("virkningsdatoish")]
        [JsonProperty("virkningsdatoish")]
        public DateTime EffectiveDateTime { get ; set; }

        [XmlElement("virkningsdato")]
        [JsonProperty("virkningsdato")]
        public string EffectiveDate
        {
            get { return EffectiveDateTime.Date.ToString("yyyy-MM-dd");
        }
            //HACK: temporary to make json serialization work
            set => throw new NotImplementedException();
        }
    }
}
