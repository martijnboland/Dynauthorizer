using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynauthorizer
{
    public class RuleConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IRule);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var rule = default(IRule);
            if (jsonObject.Properties().Any(p => p.Name.ToLowerInvariant() == "rules"))
            {
                rule = new RuleSet();
            }
            else
            {
                rule = new ClaimRule();
            }
            serializer.Populate(jsonObject.CreateReader(), rule);
            return rule;
        }
    }
}
