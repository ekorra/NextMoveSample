using System;
using System.Collections.Generic;
using System.Text;

namespace NextMove.Lib
{

    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Capabilities
    {
        [JsonProperty("capabilities")]
        public Capability[] CapabilitiesList { get; set; }
    }

    public partial class Capability
    {
        [JsonProperty("process")]
        public string Process { get; set; }

        [JsonProperty("serviceIdentifier")]
        public string ServiceIdentifier { get; set; }

        [JsonProperty("documentTypes")]
        public DocumentType[] DocumentTypes { get; set; }

        public string PrettyProcessName
        {
            get
            {
                //urn: no: difi: profile: arkivmelding: planByggOgGeodata: ver1.0

                return Process.Split(':')[5];
            }
        }
    }

    public partial class DocumentType
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("standard")]
        public string Standard { get; set; }
    }

    public partial class Capabilities
    {
        public static Capabilities FromJson(string json) => JsonConvert.DeserializeObject<Capabilities>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Capabilities self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

}
