using System.Text.Json;
using System.Text.Json.JsonDiffPatch;
using System.Text.Json.JsonDiffPatch.Xunit;
using System.Text.Json.Serialization;
using NsisoLauncherX.Core.Version;
using Xunit.Abstractions;

namespace NsisoLauncherX.Core.Test.Version;

public class VersionJsonConvertTest(ITestOutputHelper testOutputHelper)
{
    // TODO: Add mod version json file.
    [Theory]
    [InlineData("./Resources/TestVersionJsonFile/1.20.6_official_v21.json")]
    [InlineData("./Resources/TestVersionJsonFile/1.19.4_official_v21.json")]
    [InlineData("./Resources/TestVersionJsonFile/1.16.5_official_v21.json")]
    [InlineData("./Resources/TestVersionJsonFile/1.12.2_official_v18.json")]
    [InlineData("./Resources/TestVersionJsonFile/1.7.10_official_v13.json")]
    [InlineData("./Resources/TestVersionJsonFile/rd-132211_official_v7.json")]
    [InlineData("./Resources/TestVersionJsonFile/1.0_official_v4.json")]
    public async Task TestVersionJsonConvert(string path)
    {
        // Arrange
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        
        // Action
        var originalJson = await File.ReadAllTextAsync(path);
        var midJsonObject = JsonSerializer.Deserialize<VersionBase>(originalJson, options);
        var recoverJson = JsonSerializer.Serialize(midJsonObject, options);
        var diff = JsonDiffPatcher.Diff(originalJson, recoverJson);
        if (diff != null)
        {
            testOutputHelper.WriteLine("The json contains differences:");
            testOutputHelper.WriteLine(diff.ToString());
        }

        // Assert
        Assert.True(diff == null);
    }
}