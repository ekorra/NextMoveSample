using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NextMove.Lib
{
    
    public abstract class BusinessMessageCore
    {
        [XmlElement("sikkerhetsnivaa")]
        public int SecurityLevel { get; set; }

        [XmlElement("hoveddokument")]
        public string PrimaryDocumentName { get; set; }

    }
}
