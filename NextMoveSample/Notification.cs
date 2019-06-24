using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NextMove.Lib
{
    [DataContract(Name = "varsler")]
    public class Notification
    {
        [XmlElement("epostTekst")]
        public string EmailText { get; set; }

        [XmlElement("smsTekst")]
        public string SmsText { get; set; }
    }
}
