using System.Data;
using System.Text.Json.Serialization;

namespace NsisoLauncherX.Core.Version;

public class Library : IRuleConstraint, IEquatable<Library>
{
    /// <summary>
    /// Download info
    /// </summary>
    public LibraryDownloads? Downloads { get; set; }
    
    /// <summary>
    /// The extract rules, like include or exclude file list.
    /// </summary>
    public Extract? Extract { get; set; }
    
    /// <summary>
    /// The name of the library
    /// </summary>
    public required Artifact Name { get; set; }
    
    /// <summary>
    /// Native list (like dll in windows)
    /// </summary>
    public Dictionary<string, string>? Natives { get; set; }

    /// <summary>
    /// The rules to check the library enable or not
    /// </summary>
    public RuleCollection? Rules { get; set; }

    /// <summary>
    /// Download url for some strange version json (normally we should not use it)
    /// </summary>
    public string? Url { get; set; }

    public bool CheckIsEnable(Dictionary<string, bool>? givenFeatures)
    {
        return Rules == null || Rules.CheckIsEnable(givenFeatures);
    }

    public bool Equals(Library? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || Name.Descriptor.Equals(other.Name.Descriptor);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Library)obj);
    }

    public override int GetHashCode()
    {
        return Name.Descriptor.GetHashCode();
    }
}

[JsonConverter(typeof(ArtifactJsonConverter))]
public class Artifact
{
    /// <summary>
    /// 包名
    /// </summary>
    public string Package { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Classifier分类
    /// </summary>
    public string? Classifier { get; set; }

    /// <summary>
    /// 扩展名
    /// </summary>
    public string Extension { get; set; } = "jar";

    /// <summary>
    /// 原字符串
    /// </summary>
    [JsonPropertyName("name")]
    public string Descriptor { get; set; }

    [JsonConstructor]
    public Artifact(string descriptor)
    {
        this.Descriptor = descriptor;
        var parts = descriptor.Split(':');

        this.Package = parts[0];
        this.Name = parts[1];
        this.Version = parts[2];

        var last = parts.Length - 1;
        var idx = parts[last].IndexOf('@');
        if (idx != -1)
        {
            this.Extension = parts[last][(idx + 1)..];
            parts[last] = parts[last][..idx];
        }
        this.Version = parts[2];
        if (parts.Length > 3)
        {
            this.Classifier = parts[3];
        }
    }

    public string Path => Classifier == null ? string.Format(@"{0}\{1}\{2}\{1}-{2}.{3}", Package.Replace(".", "\\"), Name, Version, Extension) : string.Format(@"{0}\{1}\{2}\{1}-{2}-{3}.{4}", Package.Replace(".", "\\"), Name, Version, Classifier, Extension);

    public override string ToString()
    {
        return Descriptor;
    }
}



public class Extract
{
    /// <summary>
    /// The file path to Exculde
    /// </summary>
    public List<string>? Exclude { get; set; }
    
    /// <summary>
    /// The file path to Include
    /// </summary>
    public List<string>? Include { get; set; }
}

public class LibraryDownloads
{
    /// <summary>
    /// An artifact refers to a specific file, usually a JAR file, that represents a compiled and packaged Java library or resource.
    /// </summary>
    public PathSha1SizeUrl? Artifact { get; set; }

    /// <summary>
    /// classifiers may denote platform-specific builds, debug builds, or extra resources like source code or Javadoc files.
    /// </summary>
    public Dictionary<string, PathSha1SizeUrl>? Classifiers { get; set; }
}