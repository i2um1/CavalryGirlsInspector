using CavalryGirlsInspector.Console.Models;
using CavalryGirlsInspector.Console.Utils;

namespace CavalryGirlsInspector.Console.Repositories;

public sealed class RawItemRepository
{
    private const string FILE_NAME = "Items.txt";
    private const string FUSION = "WeaponDemo";

    private readonly string _dataFolder;

    public RawItemRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<Dictionary<int, Fusion>> GetFusions(Dictionary<int, Description> descriptions)
    {
        var rawItems = _dataFolder.ReadCsv<RawItem>(FILE_NAME);
        var result = new Dictionary<int, Fusion>();

        var index = 0;
        await foreach (var rawItem in rawItems)
        {
            if (rawItem.Type is null
                || !rawItem.Type.Equals(FUSION, StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            result[rawItem.Id] = new Fusion(index, rawItem, descriptions[rawItem.Id]);
            index++;
        }

        return result;
    }
}