namespace NsisoLauncherX.Core.Version;

public class VersionV1 : VersionBase
{
    public required string MinecraftArguments { get; set; }
    
    public override string GetJvmArguments(Dictionary<string, bool>? givenFeatures)
    {
        return "-Djava.library.path=${natives_directory} -cp ${classpath}";
    }

    public override string GetGameArguments(Dictionary<string, bool>? givenFeatures)
    {
        return MinecraftArguments;
    }
}