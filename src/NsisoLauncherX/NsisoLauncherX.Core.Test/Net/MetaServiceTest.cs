using NsisoLauncherX.Core.Net;
using NsisoLauncherX.Core.Net.Meta;

namespace NsisoLauncherX.Core.Test.Net;

public class MetaServiceTest
{
    private MetaService  Service { get; set; } = new();
    private NetRequester Requester { get; set; } = new();

    [Fact]
    public async Task TestVersionManifestV2()
    {
        var result = await this.Service.GetVersionManifestV2Async(this.Requester);
        Assert.True(result is { IsSuccess: true, Content: not null});
    }
    
    [Fact]
    [Obsolete("Obsolete, test for compatibility use.")]
    public async Task TestVersionManifestV1()
    {
        var result = await this.Service.GetVersionManifestV1Async(this.Requester);
        Assert.True(result is { IsSuccess: true, Content: not null });
    }
}