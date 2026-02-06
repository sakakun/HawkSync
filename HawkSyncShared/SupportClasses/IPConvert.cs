using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IPConvert : JsonConverter<IPAddress>
{
    public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => IPAddress.TryParse(reader.GetString(), out var ip) ? ip : null;

    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}