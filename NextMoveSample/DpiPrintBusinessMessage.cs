using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NextMove.Lib
{
    [XmlRoot("print")]
    public class DpiPrintBusinessMessage: BusinessMessageCore
    {
        [XmlElement("mottaker")]
        public Receiver Receiver { get; set; }

        [XmlElement("posttype")]
        public string PostalType { get; set; }

        [XmlElement("utskriftsfarge")]
        public string PrintColor { get; set; }

        [XmlElement("retur")]
        public ReturnPost ReturnPost { get; set; }
    }

    public class ReturnPost
    {
        [XmlElement("mottaker")]
        public Receiver Receiver { get; set; }

        [XmlElement("returhaandtering")]
        public string PostHandling { get; set; }
    }

    public class Receiver
    {
        [XmlElement("navn")]
        public string Name { get; set; }

        [XmlElement("adresselinje1")]
        public string AddressLine1 { get; set; }

        [XmlElement("adresselinje2")]
        public string AddressLine2 { get; set; }

        [XmlElement("adresselinje3")]
        public string AddressLine3 { get; set; }

        [XmlElement("adresselinje4")]
        public string AddressLine4 { get; set; }

        [XmlElement("postnummer")]
        public string ZipCode { get; set; }

        [XmlElement("poststed")]
        public string City { get; set; }

        [XmlElement("landkode")]
        public string CountryCode { get; set; }

        [XmlElement("land")]
        public string Country { get; set; }
    }
}
