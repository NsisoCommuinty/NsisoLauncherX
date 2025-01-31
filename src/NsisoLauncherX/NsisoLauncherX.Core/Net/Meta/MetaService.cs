using System.Net.Http.Json;
using System.Text.Json;

namespace NsisoLauncherX.Core.Net.Meta;

public class MetaService : IWebApi
{
    public string BaseUrl { get; set; } = "https://launchermeta.mojang.com";

    /// <summary>
    /// Get the version manifest(Ver.1) from mojang launcher meta web api.
    /// This API is NOT RECOMMEND due to the newest manifest version. Only for compatibility use.
    /// </summary>
    /// <param name="requester">Net requester instance</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>The version manifest(Ver.1)</returns>
    [Obsolete("The newest launcher should use GetVersionManifestV2 instead, GetVersionManifestV1 is for compatibility use.")]
    public async Task<RequestResult<VersionManifest<VersionMeta>>> GetVersionManifestV1Async(NetRequester requester, CancellationToken cancellation = default)
    {
        var uri = $"{BaseUrl}/mc/game/version_manifest.json";
        try
        {
            var jsonRespond = await requester.Client.GetAsync(uri, cancellation);
            if (!jsonRespond.IsSuccessStatusCode)
            {
                return RequestResult<VersionManifest<VersionMeta>>.Error(jsonRespond);
            }
            var manifest = await jsonRespond.Content.ReadFromJsonAsync<VersionManifest<VersionMeta>>(cancellation);
            return RequestResult<VersionManifest<VersionMeta>>.Success(jsonRespond, manifest);
        }
        catch (Exception e)
        {
            return RequestResult<VersionManifest<VersionMeta>>.Exception(e, uri, cancellation.IsCancellationRequested);
        }
    }
    
    /// <summary>
    /// Get the version manifest(Ver.2) from mojang launcher meta web api.
    /// </summary>
    /// <param name="requester">Net requester instance</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>The version manifest(Ver.2)</returns>
    public async Task<RequestResult<VersionManifest<VersionMetaV2>>> GetVersionManifestV2Async(NetRequester requester, CancellationToken cancellation = default)
    {
        var uri = $"{BaseUrl}/mc/game/version_manifest_v2.json";
        try
        {
            var jsonRespond = await requester.Client.GetAsync(uri, cancellation);
            if (!jsonRespond.IsSuccessStatusCode)
            {
                return RequestResult<VersionManifest<VersionMetaV2>>.Error(jsonRespond);
            }
            var manifest = await jsonRespond.Content.ReadFromJsonAsync<VersionManifest<VersionMetaV2>>(cancellation);
            return RequestResult<VersionManifest<VersionMetaV2>>.Success(jsonRespond, manifest);
        }
        catch (Exception e)
        {
            return RequestResult<VersionManifest<VersionMetaV2>>.Exception(e, uri, cancellation.IsCancellationRequested);
        }
    }
}