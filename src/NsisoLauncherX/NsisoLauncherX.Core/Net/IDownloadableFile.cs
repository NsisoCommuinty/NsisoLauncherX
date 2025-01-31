using System.Security.Cryptography;

namespace NsisoLauncherX.Core.Net;

public interface IDownloadableFile
{
    /// <summary>
    /// Get the download source url of this object
    /// </summary>
    /// <returns>Download source url</returns>
    string DownloadSourceUrl { get; }
    
    /// <summary>
    /// The total size of this file, null for unknown
    /// </summary>
    /// <returns>Download source url</returns>
    ulong? DownloadTotalSize { get; }
    
    /// <summary>
    /// The hash algorithm name and the value for this object to check, null for no check.
    /// </summary>
    /// <returns>Download source url</returns>
    Tuple<HashAlgorithmName, string>? HashNameValue { get; }
}