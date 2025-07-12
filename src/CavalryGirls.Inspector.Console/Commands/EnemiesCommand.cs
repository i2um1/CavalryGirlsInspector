using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Repositories;
using CavalryGirls.Inspector.Settings;
using CavalryGirls.Inspector.Utils;

using Spectre.Console;
using Spectre.Console.Cli;

namespace CavalryGirls.Inspector.Commands;

public sealed class EnemiesCommand : AsyncCommand<EnemiesCommandSettings>
{
    private string _outputFolder = null!;

    private RawDescriptionRepository _rawDescriptionRepository = null!;

    public override async Task<int> ExecuteAsync(CommandContext commandContext, EnemiesCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings.AssetsPath);
        ArgumentNullException.ThrowIfNull(settings.OutputPath);

        _outputFolder = settings.OutputPath.GetAssetsOutputFolder();

        var tableFolder = settings.AssetsPath.GetTableFolder();

        _rawDescriptionRepository = new RawDescriptionRepository(tableFolder);

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("Init...", Generate);

        return 0;
    }

    private async Task Generate(StatusContext context)
    {
        var enemyDescriptions = await GetEnemyDescriptions(context);

        // TODO: save enemies and images

        AnsiConsole.MarkupLine("Done");
    }

    private async Task<Dictionary<string, Description>> GetEnemyDescriptions(StatusContext context)
    {
        context.Status("Getting enemy descriptions...");
        return await _rawDescriptionRepository.GetEnemyDescriptions();
    }
}