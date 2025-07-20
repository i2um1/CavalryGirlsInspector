using CavalryGirls.Inspector.Utils;

using Spectre.Console;
using Spectre.Console.Cli;

namespace CavalryGirls.Inspector.Commands;

public abstract class CommandBase<TSettings> : AsyncCommand<TSettings>
    where TSettings : CommandSettings
{
    protected string OutputFolder = null!;

    protected async Task GenerateAtlas(
        StatusContext context, string status, string outputFileName, string[] imagePaths)
    {
        context.Status(status);
        var atlas = await ImageAtlas.Create(imagePaths, OutputFolder, outputFileName + ".webp");
        await atlas.SaveJson(OutputFolder, outputFileName + "_atlas.json");
    }

    protected async Task SaveData<T>(StatusContext context, string status, string outputFileName, T data)
    {
        context.Status(status);
        await data.SaveJson(OutputFolder, outputFileName + ".json");
    }
}