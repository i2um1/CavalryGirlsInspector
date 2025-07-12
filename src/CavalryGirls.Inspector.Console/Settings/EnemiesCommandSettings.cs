using System.ComponentModel;

using Spectre.Console.Cli;

namespace CavalryGirls.Inspector.Settings;

public sealed class EnemiesCommandSettings : CommandSettings
{
    [Description("Path to Assets folder in game")]
    [CommandOption("-a|--assets-path")]
    public string? AssetsPath { get; init; }

    [Description("Output path for generated files")]
    [CommandOption("-o|--output-path")]
    public string? OutputPath { get; init; }
}