using System.Runtime.InteropServices;
using NsisoLauncherX.Core.Utils;

namespace NsisoLauncherX.Core.Version;

public class RuleCollection : List<Rule>, IRuleConstraint
{
    public bool CheckIsEnable(Dictionary<string, bool>? givenFeatures)
    {
        return this.All(rule => rule.CheckIsEnable(givenFeatures));
    }
}

public class Rule : IRuleConstraint
{
    /// <summary>
    /// rule's action: "allow" or "disallow" are known.
    /// </summary>
    public required string Action { get; set; }

    /// <summary>
    /// Operating system info
    /// </summary>
    public OperatingSystemInfo? OS { get; set; }

    /// <summary>
    /// Allowed features
    /// </summary>
    public Dictionary<string, bool>? Features { get; set; }

    public bool CheckIsEnable(Dictionary<string, bool>? givenFeatures)
    {
        // p for postive and n for negative;
        var p = (Action == "allow");
        var n = !p;
        
        // TODO: add features check.
        // check the features
        if (Features != null)
        {
            if (givenFeatures == null)
            {
                return n;
            }
            foreach (var feature in Features)
            {
                if (!givenFeatures.TryGetValue(feature.Key, out var value) || value != true)
                {
                    return n; // If the feature is missing in givenFeatures or its value isn't true, return negative.
                }
            }
        }
        
        // check the os
        if (OS != null)
        {
            // check os name
            if (!SystemTools.IsOsPlatform(OS.Name))
            {
                return n;
            }

            //check os arch
            if (OS.Arch != null && !SystemTools.IsProcessArchitecture(OS.Arch))
            {
                return n;
            }

            // check os version
            if (OS.Version != null && !SystemTools.IsOsVersionRegex(OS.Version))
            {
                return n;
            }
        }

        return p;
    }
}

public class OperatingSystemInfo
{
    /// <summary>
    /// 系统名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Support os version
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Support os arch
    /// </summary>
    public string? Arch { get; set; }
}