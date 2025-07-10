using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Repositories;

public sealed class RawDescriptionRepository
{
    private const string FILE_NAME = "Descriptions.txt";
    private const string ITEM_NAME = "Items_Name_";
    private const string ITEM_DESCRIPTION = "Items_Description_";
    private const string ENEMY_NAME = "Enemys_Name_";
    private const string ENEMY_DESCRIPTION = "Enemys_Description_";

    private readonly string _dataFolder;

    public RawDescriptionRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<Dictionary<int, Description>> GetItemDescriptions()
    {
        var rawDescriptions = _dataFolder.ReadCsv<RawDescription>(FILE_NAME);
        var result = new Dictionary<int, Description>();

        await foreach (var rawDescription in rawDescriptions)
        {
            var data = GetDescription(rawDescription, ITEM_NAME, ITEM_DESCRIPTION);
            if (data is null || !int.TryParse(data.Value.Key, CultureInfo.InvariantCulture, out var id))
            {
                continue;
            }

            result[id] = result.TryGetValue(id, out var description)
                ? CopyDescription(data.Value.Value, description)
                : data.Value.Value;
        }

        return result;
    }

    public async Task<Dictionary<string, Description>> GetEnemyDescriptions()
    {
        var rawDescriptions = _dataFolder.ReadCsv<RawDescription>(FILE_NAME);
        var result = new Dictionary<string, Description>();

        await foreach (var rawDescription in rawDescriptions)
        {
            var data = GetDescription(rawDescription, ENEMY_NAME, ENEMY_DESCRIPTION);
            if (data is null)
            {
                continue;
            }

            result[data.Value.Key] = result.TryGetValue(data.Value.Key, out var description)
                ? CopyDescription(data.Value.Value, description)
                : data.Value.Value;
        }

        return result;
    }

    private static Description CopyDescription(Description from, Description to)
        => new(
            to.Name.Length > 0 ? to.Name : from.Name,
            to.Value.Length > 0 ? to.Value : from.Value);

    private static KeyValuePair<string, Description>? GetDescription(
        RawDescription rawDescription, string name, string description)
    {
        if (rawDescription.Id is null || rawDescription.English is null)
        {
            return null;
        }

        if (rawDescription.Id.StartsWith(name, StringComparison.InvariantCulture))
        {
            return new KeyValuePair<string, Description>(
                rawDescription.Id[name.Length..],
                new Description(rawDescription.English, string.Empty));
        }

        if (rawDescription.Id.StartsWith(description, StringComparison.InvariantCulture))
        {
            return new KeyValuePair<string, Description>(
                rawDescription.Id[description.Length..],
                new Description(string.Empty, rawDescription.English));
        }

        return null;
    }
}