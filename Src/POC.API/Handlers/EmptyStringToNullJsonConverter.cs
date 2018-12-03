using System;
using Newtonsoft.Json;

namespace POC.API.Handlers
{
    /// <summary>
    /// Converts empty and blank strings to null while serialising
    /// </summary>
    public class EmptyStringToNullJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(string) == objectType;
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            else
            {
                string value = reader.Value.ToString();
                return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }
    }
}
