using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NsisoLauncherX.Core.Net.Meta;

[DebuggerDisplay("VersionManifest count={Versions?.Count}")]
public class VersionManifest<TContent> where TContent : VersionMeta
{
    [JsonPropertyName("latest")]
    public required VersionLatest? Latest { get; init; }

    [JsonPropertyName("versions")]
    public required List<TContent>? Versions { get; init; }
}

[DebuggerDisplay("{Id}")]
public class VersionMetaV2 : VersionMeta
{
    /// <summary>
    /// The sha1 hash of the version (like id)
    /// </summary>
    [JsonPropertyName("sha1")]
    public required string Sha1 { get; init; }

    /// <summary>
    /// The compliance level of the version
    /// If 0, the launcher warns the user about this version not being recent enough to support the latest player safety features. Its value is 1 otherwise.
    /// </summary>
    [JsonPropertyName("complianceLevel")]
    public required int ComplianceLevel { get; init; }
}

[DebuggerDisplay("{Id}")]
public class VersionMeta
{
    /// <summary>
    /// 版本ID
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// 版本类型 
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// 版本修改时间
    /// </summary>
    [JsonPropertyName("time")]
    public required DateTime Time { get; init; }

    /// <summary>
    /// 版本发布时间
    /// </summary>
    [JsonPropertyName("releaseTime")]
    public required DateTime ReleaseTime { get; init; }

    /// <summary>
    /// 版本下载URL
    /// </summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}

public class VersionLatest
{
    [JsonPropertyName("release")]
    public required string Release { get; init; }

    [JsonPropertyName("snapshot")]
    public required string Snapshot { get; init; }
}