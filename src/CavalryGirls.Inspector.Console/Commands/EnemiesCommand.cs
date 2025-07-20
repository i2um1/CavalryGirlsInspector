using CavalryGirls.Inspector.Mappers;
using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Repositories;
using CavalryGirls.Inspector.Settings;
using CavalryGirls.Inspector.Utils;

using Spectre.Console;
using Spectre.Console.Cli;

namespace CavalryGirls.Inspector.Commands;

public sealed class EnemiesCommand : CommandBase<EnemiesCommandSettings>
{
    private string _enemyFolder = null!;

    private RawDescriptionRepository _rawDescriptionRepository = null!;
    private RawEnemyRepository _rawEnemyRepository = null!;

    public override async Task<int> ExecuteAsync(CommandContext commandContext, EnemiesCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings.AssetsPath);
        ArgumentNullException.ThrowIfNull(settings.OutputPath);

        OutputFolder = settings.OutputPath.GetAssetsOutputFolder();
        _enemyFolder = settings.AssetsPath.GetEnemyFolder();

        var tableFolder = settings.AssetsPath.GetTableFolder();

        _rawDescriptionRepository = new RawDescriptionRepository(tableFolder);
        _rawEnemyRepository = new RawEnemyRepository(tableFolder);

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("Init...", Generate);

        return 0;
    }

    private async Task Generate(StatusContext context)
    {
        var enemyDescriptions = await GetEnemyDescriptions(context);
        context.Status("Getting enemies...");

        var (enemies, invalidEnemies) = await _rawEnemyRepository.GetEnemies(enemyDescriptions);
        AnsiConsole.MarkupLine($"[red]Failed to find descriptions:[/] {string.Join(", ", invalidEnemies)}");
        enemies = ReindexEnemies(enemies);

        await SaveData(context, "Saving enemies...", "enemies", enemies.Values);

        var imagePaths = enemies.ToImagePaths(_enemyFolder);
        await GenerateAtlas(context, "Saving enemies atlas...", "enemies", imagePaths);

        AnsiConsole.MarkupLine("Done");
    }

    private Dictionary<string, Enemy> ReindexEnemies(
        Dictionary<string, Enemy> enemies)
    {
        var invalidEnemies = FindInvalidEnemies(enemies);
        AnsiConsole.MarkupLine($"[red]Failed to find image:[/] {string.Join(", ", invalidEnemies)}");

        return enemies.Values
            .Where(x => !invalidEnemies.Contains(x.Id))
            .Select((enemy, index) =>
            {
                enemy.Index = index;
                return enemy;
            })
            .ToDictionary(x => x.Id, x => x);
    }

    private HashSet<string> FindInvalidEnemies(Dictionary<string, Enemy> enemies)
        => enemies.Keys
            .Where(id => !File.Exists(id.ToImagePath(_enemyFolder)))
            .ToHashSet();

    private async Task<Dictionary<string, Description>> GetEnemyDescriptions(StatusContext context)
    {
        context.Status("Getting enemy descriptions...");
        return await _rawDescriptionRepository.GetEnemyDescriptions();
    }
}