using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Repositories;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Mappers;

public static class WeaponTypeMappers
{
    private static readonly Dictionary<string, WeaponSubType> _subWeaponTypes = new()
    {
        ["All"] = WeaponSubType.All,
        ["NormalWeapon"] = WeaponSubType.Kinetic,
        ["Riffle"] = WeaponSubType.Sniper,
        ["MG"] = WeaponSubType.MachineGun,
        ["Rail"] = WeaponSubType.GuideRail,
        ["ExploWeapon"] = WeaponSubType.Explosive,
        ["FlakeWeapon"] = WeaponSubType.Plasma,
        ["ShotWeapon"] = WeaponSubType.Spreadshot,
        ["SplitWeapon"] = WeaponSubType.Spraying,
        ["LaserWeapon"] = WeaponSubType.Ray,
        ["ElectricWeapon"] = WeaponSubType.Arc,
        ["MagWeapon"] = WeaponSubType.Magnetoelectric,
        ["TechWeapon"] = WeaponSubType.Tech
    };

    private static readonly Dictionary<string, WeaponSubType> _closeSubWeaponTypes = new()
    {
        ["All"] = WeaponSubType.All,
        ["MeeleeWeapon"] = WeaponSubType.CloseIn,
        ["FistWeapon"] = WeaponSubType.Boxing,
        ["SaberWeapon"] = WeaponSubType.Sword,
        ["AxeWeapon"] = WeaponSubType.Axe,
        ["SpearWeapon"] = WeaponSubType.Spear,
        ["Dagger"] = WeaponSubType.Dagger,
        ["RiotShiled"] = WeaponSubType.Shield
    };

    private static readonly Dictionary<string, WeaponSubType> _hangShoulderSubWeaponTypes = new()
    {
        ["All"] = WeaponSubType.All,
        ["NormalWeapon"] = WeaponSubType.Kinetic,
        ["ExploWeapon"] = WeaponSubType.Explosive,
        ["RocketWeapon"] = WeaponSubType.Rocket,
        ["EMP"] = WeaponSubType.EMP,
        ["AntiAir"] = WeaponSubType.AntiAir,
        ["LaserWeapon"] = WeaponSubType.Ray
    };

    public static WeaponType ConvertFusionToWeaponType(this string? fusionType)
        => fusionType switch
        {
            null => throw new ArgumentNullException(nameof(fusionType)),
            RawItemRepository.CLOSE_WEAPON_FUSION => WeaponType.Close,
            RawItemRepository.HANG_SHOULDER_FUSION => WeaponType.HangShoulder,
            _ => WeaponType.Weapon
        };

    public static WeaponType ConvertModuleToWeaponType(this string? moduleType)
        => moduleType switch
        {
            null => throw new ArgumentNullException(nameof(moduleType)),
            RawItemRepository.CLOSE_WEAPON_MODULE => WeaponType.Close,
            RawItemRepository.HANG_SHOULDER_MODULE => WeaponType.HangShoulder,
            _ => WeaponType.Weapon
        };

    public static WeaponType ConvertWeaponToWeaponType(this string? weaponType)
        => weaponType switch
        {
            null => throw new ArgumentNullException(nameof(weaponType)),
            RawItemRepository.CLOSE_WEAPON => WeaponType.Close,
            RawItemRepository.HANG_SHOULDER => WeaponType.HangShoulder,
            _ => WeaponType.Weapon
        };

    public static WeaponSubType[] ToWeaponSubTypes(this string? tag, WeaponType weaponType)
    {
        var subTypes = GetWeaponSubTypes(weaponType);
        return tag
            .SplitStringOr()
            .Where(x => subTypes.ContainsKey(x))
            .Select(x => subTypes[x])
            .Distinct()
            .OrderBy(x => x)
            .ToArray();
    }

    public static WeaponSubTypeCondition[] ToFusionWeaponSubTypeConditions(
        this string? conditions, WeaponType weaponType)
    {
        var subTypes = GetWeaponSubTypes(weaponType);
        return conditions
            .SplitStringOr()
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(type =>
            {
                var conditionType = type.StartsWith('!') ? ConditionType.Exclude : ConditionType.Include;
                if (conditionType is ConditionType.Exclude)
                {
                    type = type[1..];
                }

                if (!subTypes.TryGetValue(type, out var subType))
                {
                    throw new InvalidDataException($"Unknown weapon sub type: {type}");
                }

                return new WeaponSubTypeCondition(conditionType, subType);
            })
            .ToArray();
    }

    private static Dictionary<string, WeaponSubType> GetWeaponSubTypes(WeaponType weaponType)
        => weaponType switch
        {
            WeaponType.Close => _closeSubWeaponTypes,
            WeaponType.HangShoulder => _hangShoulderSubWeaponTypes,
            _ => _subWeaponTypes
        };
}