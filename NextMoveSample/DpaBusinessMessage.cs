using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NextMove.Lib
{
    [XmlRoot("avtalt")]
    public class DpaBusinessMessage: BusinessMessageCore
    {
        [JsonProperty("content")]
        public string content { get; set; }

        [JsonProperty("identifier")]
        public string identifier;
    }
}
