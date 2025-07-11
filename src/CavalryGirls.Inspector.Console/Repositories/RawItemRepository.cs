using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

using static CavalryGirls.Inspector.Mappers.ItemMappers;

namespace CavalryGirls.Inspector.Repositories;

public sealed class RawItemRepository
{
    public const string CLOSE_WEAPON_FUSION = "CloseWeaponDemo";
    public const string HANG_SHOULDER_FUSION = "HangShoulderDemo";

    public const string CLOSE_WEAPON_MODULE = "CloseWeaponModel";
    public const string HANG_SHOULDER_MODULE = "HangShoulderModel";

    public const string CLOSE_WEAPON = "CloseWeapon";
    public const string HANG_SHOULDER = "HangShoulder";

    private const string FILE_NAME = "Items.txt";

    private const string MATERIAL = "Material";

    private readonly string _dataFolder;

    private readonly string[] _fusions = ["WeaponDemo", CLOSE_WEAPON_FUSION, HANG_SHOULDER_FUSION];
    private readonly string[] _weaponModules = ["WeaponModel", CLOSE_WEAPON_MODULE, HANG_SHOULDER_MODULE];
    private readonly string[] _weapons = ["Weapon", CLOSE_WEAPON, HANG_SHOULDER];

    public RawItemRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<Dictionary<int, Material>> GetMaterials(Dictionary<int, Description> descriptions)
        => await GetItem([MATERIAL], descriptions, ToMaterial);

    public async Task<Dictionary<int, Fusion>> GetFusions(Dictionary<int, Description> descriptions)
        => await GetItem(_fusions, descriptions, ToFusion);

    public async Task<Dictionary<int, WeaponModule>> GetWeaponModules(Dictionary<int, Description> descriptions)
        => await GetItem(_weaponModules, descriptions, ToWeaponModule);

    private async Task<Dictionary<int, T>> GetItem<T>(
        string[] itemTypes,
        Dictionary<int, Description> descriptions,
        Func<int, RawItem, Description, T> map)
    {
        var rawItems = _dataFolder.ReadCsv<RawItem>(FILE_NAME);
        var result = new Dictionary<int, T>();

        var index = 0;
        await foreach (var rawItem in rawItems)
        {
            if (rawItem.Type is null || !itemTypes.Contains(rawItem.Type, StringComparer.InvariantCulture))
            {
                continue;
            }

            result[rawItem.Id] = map(index, rawItem, descriptions[rawItem.Id]);
            index++;
        }

        return result;
    }
}