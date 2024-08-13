using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeHelper.Model.Serialize
{
    public class LongArrayToStringArrayConverter : JsonConverter<List<long>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override List<long> Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            List<long> result = new List<long>();
            string? jsonString = reader.GetString();
            if (!string.IsNullOrEmpty(jsonString))
            {
                List<string>? values = JsonSerializer.Deserialize<List<string>>(jsonString);
                if (values != null)
                {
                    foreach (string value in values)
                    {
                        if (long.TryParse(value, out long v))
                        {
                            result.Add(v);
                        }
                        else
                        {
                            result.Add(0);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, List<long> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (long i in value)
            {
                writer.WriteStringValue(i.ToString());
            }
            writer.WriteEndArray();
            writer.Flush();
        }
    }
}
