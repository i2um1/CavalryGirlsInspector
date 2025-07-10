using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Repositories;

public sealed class RawItemRepository
{
    private const string FILE_NAME = "Items.txt";
    private const string FUSION = "WeaponDemo";
    private const string WEAPON_MODULE = "WeaponModel";

    private readonly string _dataFolder;

    public RawItemRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<Dictionary<int, Fusion>> GetFusions(Dictionary<int, Description> descriptions)
        => await GetItem(FUSION, descriptions, CreateFusion);

    public async Task<Dictionary<int, WeaponModule>> GetWeaponModules(Dictionary<int, Description> descriptions)
        => await GetItem(WEAPON_MODULE, descriptions, CreateWeaponModule);

    private async Task<Dictionary<int, T>> GetItem<T>(
        string itemType,
        Dictionary<int, Description> descriptions,
        Func<int, RawItem, Description, T> createItem)
    {
        var rawItems = _dataFolder.ReadCsv<RawItem>(FILE_NAME);
        var result = new Dictionary<int, T>();

        var index = 0;
        await foreach (var rawItem in rawItems)
        {
            if (rawItem.Type is null
                || !rawItem.Type.Equals(itemType, StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            result[rawItem.Id] = createItem(index, rawItem, descriptions[rawItem.Id]);
            index++;
        }

        return result;
    }

    private static Fusion CreateFusion(int index, RawItem item, Description description)
    {
        var craft = item.Craft.Split('+');
        return new Fusion
        {
            Index = index,
            Id = item.Id,
            Name = description.Name,
            Description = description.Value,
            Functions = GetFunctions(item.Functions),
            Price = item.Price,
            Day = item.Day ?? throw new ArgumentNullException(item.Day),
            Level = item.Level,
            Family = GetFamily(item.Family),
            BaseWeapons = GetBaseWeapons(item.Craft),
            FusionWeapons = GetFusionWeapons(item.Craft)
        };
    }

    private static WeaponModule CreateWeaponModule(int index, RawItem item, Description description)
    {
        var craft = item.Craft.Split('+');
        return new WeaponModule
        {
            Index = index,
            Id = item.Id,
            Name = description.Name,
            Description = description.Value,
            Raw = item
        };
    }

    private static int[] GetFamily(string? family)
    {
        ArgumentNullException.ThrowIfNull(family);

        return family
            .Split('|')
            .Select(x => int.Parse(x, CultureInfo.InvariantCulture))
            .ToArray();
    }

    private static Function[] GetFunctions(string? functions)
    {
        ArgumentNullException.ThrowIfNull(functions);

        return functions
            .Split('|')
            .Select(x =>
            {
                var parts = x.Split(':');
                return new Function(parts[0], parts[1]);
            })
            .ToArray();
    }

    private static string[] GetBaseWeapons(string? craft)
    {
        ArgumentNullException.ThrowIfNull(craft);

        return craft
            .Split('+')
            .First()
            .Split('|')
            .ToArray();
    }

    private static string[] GetFusionWeapons(string? craft)
    {
        ArgumentNullException.ThrowIfNull(craft);

        return craft
            .Split('+')
            .Last()
            .Split('|')
            .ToArray();
    }
}