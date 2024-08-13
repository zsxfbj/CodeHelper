using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CodeHelper.Model.Serialize
{
    /// <summary>
    /// Decimal数据类JSON序列化和反序列化的约定
    /// </summary>
    public class DecimalConverter : JsonConverter<decimal>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            decimal.TryParse(reader.GetString(), out decimal value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("#0.####"));
        }
    }

}
