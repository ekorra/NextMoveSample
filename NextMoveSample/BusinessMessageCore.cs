using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NextMove.Lib
{
    
    public abstract class BusinessMessageCore
    {
        [XmlElement("sikkerhetsnivaa")]
        [JsonProperty("sikkerhetsnivaa")]
        public int SecurityLevel { get; set; }

        [XmlElement("hoveddokument")]
        [JsonProperty("hoveddokument")]
        public string PrimaryDocumentName { get; set; }

    }
}
