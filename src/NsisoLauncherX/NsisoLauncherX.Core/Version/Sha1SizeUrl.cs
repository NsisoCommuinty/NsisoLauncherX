using System.Security.Cryptography;
using System.Text.Json.Serialization;
using NsisoLauncherX.Core.Net;

namespace NsisoLauncherX.Core.Version;

/// <summary>
/// The basic element for sha1, size and url model
/// </summary>
public class Sha1SizeUrl : IDownloadableFile
{
    /// <summary>
    /// SHA1
    /// </summary>
    public required string Sha1 { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public required ulong Size { get; set; }

    /// <summary>
    /// 下载URL
    /// </summary>
    public required string Url { get; set; }

    #region IDownloadableFile implement
    // implement for IDownloadableFile interface
    [JsonIgnore]
    public string DownloadSourceUrl => Url;
    
    [JsonIgnore]
    public ulong? DownloadTotalSize => Size;
    
    [JsonIgnore]
    public Tuple<HashAlgorithmName, string>? HashNameValue => new(HashAlgorithmName.SHA1, Sha1);
    #endregion
}

/// <summary>
/// The basic element for path, sha1, size and url model
/// </summary>
public class PathSha1SizeUrl : Sha1SizeUrl
{
    /// <summary>
    /// The file path
    /// </summary>
    public required string Path { get; set; }
}

/// <summary>
/// The basic element for id, sha1, size and url model
/// </summary>
public class IdSha1SizeUrl : Sha1SizeUrl
{
    /// <summary>
    /// The file path
    /// </summary>
    public required string Id { get; set; }
}