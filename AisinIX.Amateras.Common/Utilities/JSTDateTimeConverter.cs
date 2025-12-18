using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AisinIX.Amateras.Common.Utilities
{
    public class JSTDateTimeConverter: JsonConverter<DateTime>
    {
        private static readonly TimeZoneInfo jstZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var d = DateTime.Parse(reader.GetString());
            if (d.Kind == DateTimeKind.Utc)
            {
                d = System.TimeZoneInfo.ConvertTimeFromUtc(d, jstZoneInfo);
            }
            return d;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                value = System.TimeZoneInfo.ConvertTimeFromUtc(value, jstZoneInfo);
            }
            writer.WriteStringValue(value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss+09:00"));
        }
    }
}