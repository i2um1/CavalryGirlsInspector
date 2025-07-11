using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

using static CavalryGirls.Inspector.Mappers.ItemMappers;

namespace CavalryGirls.Inspector.Repositories;

public sealed class RawItemRepository
{
    private const string FILE_NAME = "Items.txt";
    private const string MATERIAL = "Material";
    private const string FUSION = "WeaponDemo";
    private const string WEAPON_MODULE = "WeaponModel";

    private readonly string _dataFolder;

    public RawItemRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<Dictionary<int, Material>> GetMaterials(Dictionary<int, Description> descriptions)
        => await GetItem(MATERIAL, descriptions, ToMaterial);

    public async Task<Dictionary<int, Fusion>> GetFusions(Dictionary<int, Description> descriptions)
        => await GetItem(FUSION, descriptions, ToFusion);

    public async Task<Dictionary<int, WeaponModule>> GetWeaponModules(Dictionary<int, Description> descriptions)
        => await GetItem(WEAPON_MODULE, descriptions, ToWeaponModule);

    private async Task<Dictionary<int, T>> GetItem<T>(
        string itemType,
        Dictionary<int, Description> descriptions,
        Func<int, RawItem, Description, T> map)
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

            result[rawItem.Id] = map(index, rawItem, descriptions[rawItem.Id]);
            index++;
        }

        return result;
    }
}