using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynauthorizer
{
    public class RuleEnumerableConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<IRule>);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var rulesArray = JArray.Load(reader);
            serializer.Converters.Add(new RuleConverter());
            var rules = rulesArray.Select(t => t.ToObject<IRule>(serializer));
            return rules;
        }
    }
}
