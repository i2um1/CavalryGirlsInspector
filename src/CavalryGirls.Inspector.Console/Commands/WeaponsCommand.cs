using CavalryGirls.Inspector.Mappers;
using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Repositories;
using CavalryGirls.Inspector.Settings;
using CavalryGirls.Inspector.Utils;

using Spectre.Console;
using Spectre.Console.Cli;

namespace CavalryGirls.Inspector.Commands;

public sealed class WeaponsCommand : AsyncCommand<WeaponsCommandSettings>
{
    private string _outputFolder = null!;
    private string _propertyFolder = null!;

    private RawBulletRepository _rawBulletRepository = null!;
    private RawDescriptionRepository _rawDescriptionRepository = null!;
    private RawItemRepository _rawItemRepository = null!;

    public override async Task<int> ExecuteAsync(CommandContext commandContext, WeaponsCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings.AssetsPath);
        ArgumentNullException.ThrowIfNull(settings.OutputPath);

        _outputFolder = settings.OutputPath.GetAssetsOutputFolder();
        _propertyFolder = settings.AssetsPath.GetPropertyFolder();

        var tableFolder = settings.AssetsPath.GetTableFolder();

        _rawBulletRepository = new RawBulletRepository(tableFolder);
        _rawDescriptionRepository = new RawDescriptionRepository(tableFolder);
        _rawItemRepository = new RawItemRepository(tableFolder);

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("Init...", Generate);

        return 0;
    }

    private async Task Generate(StatusContext context)
    {
        var itemDescriptions = await GetItemDescriptions(context);

        await Generate(
            context, "materials", "materials",
            async () => await _rawItemRepository.GetMaterials(itemDescriptions),
            data => data.ToImagePaths(_propertyFolder));

        await Generate(
            context, "weapon fusions", "fusions",
            async () => await _rawItemRepository.GetFusions(itemDescriptions),
            data => data.ToImagePaths(_propertyFolder));

        await Generate(
            context, "weapon modules", "modules",
            async () => await _rawItemRepository.GetWeaponModules(itemDescriptions),
            data => data.ToImagePaths(_propertyFolder));

        await Generate(
            context, "weapons", "weapons",
            async () => await _rawItemRepository.GetWeapons(itemDescriptions),
            data => data.ToImagePaths(_propertyFolder));

        await Generate(
            context, "bullets", "bullets",
            async () => await _rawBulletRepository.GetBullets());

        AnsiConsole.MarkupLine("Done");
    }

    private async Task<Dictionary<int, Description>> GetItemDescriptions(StatusContext context)
    {
        context.Status("Getting item descriptions...");
        return await _rawDescriptionRepository.GetItemDescriptions();
    }

    private async Task Generate<T>(
        StatusContext context,
        string title,
        string outputFileName,
        Func<Task<Dictionary<int, T>>> getData,
        Func<Dictionary<int, T>, string[]> getImagePaths)
    {
        var data = await Generate(context, title, outputFileName, getData);

        var imagePaths = getImagePaths(data);
        await GenerateAtlas(context, $"Saving {title} atlas...", outputFileName, imagePaths);
    }

    private async Task<Dictionary<int, T>> Generate<T>(
        StatusContext context,
        string title,
        string outputFileName,
        Func<Task<Dictionary<int, T>>> getData)
    {
        context.Status($"Getting {title}...");

        var data = await getData();
        await SaveData(context, $"Saving {title}...", outputFileName, data);

        return data;
    }

    private async Task GenerateAtlas(
        StatusContext context, string status, string outputFileName, string[] imagePaths)
    {
        context.Status(status);
        var atlas = await ImageAtlas.Create(imagePaths, _outputFolder, outputFileName + ".webp");
        await atlas.SaveJson(_outputFolder, outputFileName + "_atlas.json");
    }

    private async Task SaveData<T>(StatusContext context, string status, string outputFileName, T data)
    {
        context.Status(status);
        await data.SaveJson(_outputFolder, outputFileName + ".json");
    }
}