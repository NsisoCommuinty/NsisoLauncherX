using System.Text.Json.Serialization;

namespace NsisoLauncherX.Core.Version;

/// <summary>
/// All version file's shared info
/// </summary>
[JsonConverter(typeof(VersionJsonConverter))]
public abstract class VersionBase
{
    /// <summary>
    /// asset file index info
    /// </summary>
    public AssetIndex? AssetIndex { get; set; }

    /// <summary>
    /// using assets id
    /// </summary>
    public string? Assets { get; set; }

    /// <summary>
    /// Compliance level (for safe check)
    /// 0 until 1.16.4-pre2, and 1 for all versions after.
    /// A value of 0 causes the official launcher to warn the player about missing player safety features when this version is selected.
    /// </summary>
    public int? ComplianceLevel { get; set; }

    /// <summary>
    /// version core jar download info
    /// </summary>
    public CoreDownloads? Downloads { get; set; }

    /// <summary>
    /// the id of this version
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// This version inherits from (depends on) the given version, normally appears in the mod version like forge.
    /// Normally this would not be a recursion
    /// </summary>
    public string? InheritsFrom { get; set; }

    /// <summary>
    /// The version using java version info
    /// </summary>
    public JavaDependent? JavaVersion { get; set; }

    /// <summary>
    /// The target jar file, only appear in other non-official version.
    /// </summary>
    public string? Jar { get; set; }

    /// <summary>
    /// All the dependent libraries (artifact and classifier)
    /// </summary>
    public required List<Library> Libraries { get; set; }
    
    /// <summary>
    ///  Information about Log4j log configuration.
    /// </summary>
    public LoggingInformation? Logging { get; set; }

    /// <summary>
    /// Giving launch main class in jar
    /// for modern versions, it is net.minecraft.client.main.Main, but it may differ for older or inherit versions
    /// </summary>
    public required string MainClass { get; set; }

    /// <summary>
    /// This minimum launcher version can success launch this game. 
    /// </summary>
    public int? MinimumLauncherVersion { get; set; }

    /// <summary>
    /// Release time in ISO-8601 extended offset format.
    /// </summary>
    [JsonConverter(typeof(UtcDateTimeJsonConverter))]
    public required DateTime ReleaseTime { get; set; }
    
    /// <summary>
    /// Last update time in ISO-8601 extended offset format.
    /// </summary>
    [JsonConverter(typeof(UtcDateTimeJsonConverter))]
    public required DateTime Time { get; set; }

    /// <summary>
    /// The type of this game version.
    /// Default is one of "release", "snapshot", "old_beta" or "old_alpha"
    /// </summary>
    public required string Type { get; set; }
    
    public virtual List<Library> GetEnabledLibraries(Dictionary<string, bool>? givenFeatures)
    {
        // TODO: add different version of same library handle process. 
        return this.Libraries.Where(item => item.CheckIsEnable(givenFeatures)).ToList();
    }

    public abstract string? GetJvmArguments(Dictionary<string, bool>? givenFeatures);

    public abstract string? GetGameArguments(Dictionary<string, bool>? givenFeatures);
}

public class AssetIndex : Sha1SizeUrl
{
    /// <summary>
    /// The assets version
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Total size of all the assets file.
    /// </summary>
    public required ulong TotalSize { get; set; }
}

public class CoreDownloads
{
    /// <summary>
    /// Client core download info
    /// </summary>
    public required Sha1SizeUrl Client { get; set; }
    
    /// <summary>
    /// The obfuscation maps for this client version.
    /// Added in Java Edition 19w36a but got included in 1.14.4 also. Repeats the structure of the client download information.
    /// </summary>
    [JsonPropertyName("client_mappings")]
    public Sha1SizeUrl? ClientMappings { get; set; }

    /// <summary>
    /// Server core download info
    /// </summary>
    public Sha1SizeUrl? Server { get; set; }
    
    /// <summary>
    /// The Windows server download information.
    /// Removed in Java Edition 16w05a, but is still present in prior versions. Repeats the structure of the client download information.
    /// </summary>
    [JsonPropertyName("windows_server")]
    public Sha1SizeUrl? WindowsServer { get; set; }
    
    /// <summary>
    /// The obfuscation maps for this server version.
    /// Added in Java Edition 19w36a but got included in 1.14.4 also. Repeats the structure of the client download information.
    /// </summary>
    [JsonPropertyName("server_mappings")]
    public Sha1SizeUrl? ServerMappings { get; set; }
}

public class JavaDependent
{
    /// <summary>
    /// The java's name
    ///  Its value is "jre-legacy" until 21w18a, "java-runtime-alpha" until 1.18-pre1, "java-runtime-beta" until 22w17a, "java-runtime-gamma" until 24w13a, and "java-runtime-delta" since 24w14a.
    /// </summary>
    public required string Component { get; set; }
    
    /// <summary>
    /// The major version of this java.
    /// Its value is 8 until 21w18a, 16 until 1.18-pre1, 17 until 24w13a, and 21 since 24w14a.
    /// </summary>
    public required int MajorVersion { get; set; }
}

public class LoggingInformation
{
    public required LoggingDetails Client { get; set; }
}

public class LoggingDetails
{
    /// <summary>
    /// The JVM argument for adding the log configuration.
    /// Its value default is "-Dlog4j.configurationFile=${path}".
    /// </summary>
    public required string Argument { get; set; }

    /// <summary>
    /// The Log4j2 XML configuration used by this version for the launcher for launcher's log screen.
    /// </summary>
    public required IdSha1SizeUrl File { get; set; }

    /// <summary>
    ///  Its value default is "log4j2-xml".
    /// </summary>
    public required string Type { get; set; }
}