using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NextMove.Lib
{
    public class JsonCustomSerializer : JsonConverter
    {
        
        public override bool CanConvert(Type objectType)
        {
            // this converter can be applied to any type
            return objectType == typeof(StandardBusinessDocument);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jToken = JToken.FromObject(value);

            if (jToken.Type != JTokenType.Object)
            {
                jToken.WriteTo(writer);
            }
            else
            {
                var jObject = (JObject)jToken;

                if (!(jObject.SelectToken("Any").Children().First() is JProperty businessMessage)) return;

                var xmlAnyElement = businessMessage.Children().First().AsEnumerable();

                ///HACK: need to replace Any element with named element
                RemoveXmlNamespaces(xmlAnyElement);
                RenameAnyObjectToBusniessMessageName(jObject, businessMessage);
                jObject.WriteTo(writer);
            }
            
        }

        private static void RenameAnyObjectToBusniessMessageName(JObject jObject, JProperty businessMessage)
        {
            jObject.Add(new JProperty(businessMessage.Name, businessMessage.Children().First()));
            jObject.Remove("Any");
        }

        private static void RemoveXmlNamespaces(IEnumerable<JToken> xmlAnyElement)
        {
            var namespaceObjects = xmlAnyElement.Where(y => ((JProperty) y).Name.Contains("@xmlns")).ToArray();
            foreach (var token in namespaceObjects)
            {
                var jProperty = (JProperty) token;
                jProperty.Remove();
            }
        }
    }

}
