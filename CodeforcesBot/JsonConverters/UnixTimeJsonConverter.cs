using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeforcesBot.JsonConverters;

public sealed class UnixTimeJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long time = reader.GetInt64();
        return DateTimeOffset.FromUnixTimeSeconds(time);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.ToUnixTimeSeconds());
    }
}
