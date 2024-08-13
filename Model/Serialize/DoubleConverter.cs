using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeHelper.Model.Serialize
{
    /// <summary>
    ///  Double数据类JSON序列化和反序列化的约定
    /// </summary>
    public class DoubleConverter : JsonConverter<double>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override double Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            double.TryParse(reader.GetString(), out double value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("#0.###"));
        }
    }

}
