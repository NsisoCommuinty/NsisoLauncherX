using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace NsisoLauncherX.Core.Version;

public class VersionJsonConverter : JsonConverter<VersionBase>
{
    public override VersionBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;
        
        if (rootElement.TryGetProperty("arguments", out var v1ArgumentJsonElement) && v1ArgumentJsonElement.ValueKind == JsonValueKind.Object)
        {
            var v2 = rootElement.Deserialize<VersionV2>(options);
            return v2;
        }
        if (rootElement.TryGetProperty("minecraftArguments", out var v2ArgumentJsonElement) && v2ArgumentJsonElement.ValueKind == JsonValueKind.String)
        {
            var v1 = rootElement.Deserialize<VersionV1>(options);
            return v1;
        }
        else
        {
            throw new Exception("Unsupported version type or invalid version argument value kind.");
        }
    }

    public override void Write(Utf8JsonWriter writer, VersionBase value, JsonSerializerOptions options)
    {
        // Use type-specific serialization
        switch (value)
        {
            case VersionV1 v1:
                JsonSerializer.Serialize(writer, v1, options);
                break;
            case VersionV2 v2:
                JsonSerializer.Serialize(writer, v2, options);
                break;
            default:
                throw new JsonException("Unknown type for VersionBase.");
        }
    }
}

public class ArtifactJsonConverter : JsonConverter<Artifact>
{
    public override Artifact? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var name = reader.GetString();
        return name == null ? null : new Artifact(name);
    }

    public override void Write(Utf8JsonWriter writer, Artifact value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Descriptor);
    }
}

public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"));
    }
}