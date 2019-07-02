using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NextMove.Lib
{

    public class JsonAnySerializer : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            // this converter can be applied to any type
            return objectType == typeof(StandardBusinessDocument);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // we currently support only writing of JSON
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
                var jObject = (JObject) jToken;

                if (!(jObject.SelectToken("Any").Children().First() is JProperty businessMessage)) return;

                var xmlAnyElement = businessMessage.Children().First().AsEnumerable();

                
                jObject.WriteTo(writer);
            }

        }
    }

    public class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }
            return new List<T> { token.ToObject<T>() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Scope[] x = value as Scope[];
            var list = x.ToList<Scope>();
            if (list.Count == 1)
            {
                value = list[0];
            }
            serializer.Serialize(writer, value);
        }

        public override bool CanWrite
        {
            get { return true; }
        }
    }


    public class JsonCustomSerializer : JsonConverter
    {
        
        public override bool CanConvert(Type objectType)
        {
            // this converter can be applied to any type
            return objectType == typeof(StandardBusinessDocument);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // we currently support only writing of JSON
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
