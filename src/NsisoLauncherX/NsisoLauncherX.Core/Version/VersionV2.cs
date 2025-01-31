using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NsisoLauncherX.Core.Version;

public class VersionV2 : VersionBase
{
    public required VersionArguments Arguments { get; set; }

    public override string? GetJvmArguments(Dictionary<string, bool>? givenFeatures)
    {
        return BuildV2ArgFromJsonNodeList(Arguments.Jvm, givenFeatures);
    }

    public override string? GetGameArguments(Dictionary<string, bool>? givenFeatures)
    {
        return BuildV2ArgFromJsonNodeList(Arguments.Game, givenFeatures);
    }

    private static string? BuildV2ArgFromJsonNodeList(List<JsonNode>? args, Dictionary<string, bool>? givenFeatures)
    {
        var argBuilder = new StringBuilder();
        if (args == null)
        {
            return null;
        }

        foreach (var arg in args)
        {
            switch (arg.GetValueKind())
            {
                case JsonValueKind.Object:
                {
                    var argObj = arg.AsObject();

                    var rules = argObj["rules"].Deserialize<RuleCollection>();
                    var valueNode = argObj["value"];
                    var valueNodeType = valueNode?.GetValueKind();

                    if (rules != null && valueNode != null)
                    {
                        if (!rules.CheckIsEnable(givenFeatures))
                        {
                            continue;
                        }

                        switch (valueNodeType)
                        {
                            case JsonValueKind.String:
                            {
                                var value = valueNode.GetValue<string>();
                                if (value.Contains(' '))
                                {
                                    argBuilder.Append($"\"{value}\" ");
                                }
                                else
                                {
                                    argBuilder.Append($"{value} ");
                                }

                                break;
                            }
                            case JsonValueKind.Array:
                            {
                                var values = valueNode.GetValue<List<string>>();
                                foreach (var value in values)
                                {
                                    if (value.Contains(' '))
                                    {
                                        argBuilder.Append($"\"{value}\" ");
                                    }
                                    else
                                    {
                                        argBuilder.Append($"{value} ");
                                    }
                                }

                                break;
                            }
                            default:
                                continue;
                        }
                    }

                    break;
                }

                case JsonValueKind.String:
                {
                    var value = arg.GetValue<string>();
                    argBuilder.AppendFormat(value.Contains(' ') ? "\"{0}\" " : "{0} ", arg);
                    break;
                }
                default:
                    continue;
            }
        }

        return argBuilder.ToString().Trim();
    }
}

public class VersionArguments
{
    public List<JsonNode>? Game { get; set; }

    public List<JsonNode>? Jvm { get; set; }
}