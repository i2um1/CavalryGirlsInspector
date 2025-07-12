using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Repositories;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Mappers;

public static class WeaponTypeMappers
{
    private static readonly Dictionary<string, SubWeaponType> _subWeaponTypes = new()
    {
        ["All"] = SubWeaponType.All,
        ["NormalWeapon"] = SubWeaponType.Kinetic,
        ["Riffle"] = SubWeaponType.Sniper,
        ["MG"] = SubWeaponType.MachineGun,
        ["Rail"] = SubWeaponType.GuideRail,
        ["ExploWeapon"] = SubWeaponType.Explosive,
        ["FlakeWeapon"] = SubWeaponType.Plasma,
        ["ShotWeapon"] = SubWeaponType.Spreadshot,
        ["SplitWeapon"] = SubWeaponType.Spraying,
        ["LaserWeapon"] = SubWeaponType.Ray,
        ["ElectricWeapon"] = SubWeaponType.Arc,
        ["MagWeapon"] = SubWeaponType.Magnetoelectric,
        ["TechWeapon"] = SubWeaponType.Tech
    };

    private static readonly Dictionary<string, SubWeaponType> _closeSubWeaponTypes = new()
    {
        ["All"] = SubWeaponType.All,
        ["MeeleeWeapon"] = SubWeaponType.CloseIn,
        ["FistWeapon"] = SubWeaponType.Boxing,
        ["SaberWeapon"] = SubWeaponType.Sword,
        ["AxeWeapon"] = SubWeaponType.Axe,
        ["SpearWeapon"] = SubWeaponType.Spear,
        ["Dagger"] = SubWeaponType.Dagger,
        ["RiotShiled"] = SubWeaponType.Shield
    };

    private static readonly Dictionary<string, SubWeaponType> _hangShoulderSubWeaponTypes = new()
    {
        ["All"] = SubWeaponType.All,
        ["NormalWeapon"] = SubWeaponType.Kinetic,
        ["ExploWeapon"] = SubWeaponType.Explosive,
        ["RocketWeapon"] = SubWeaponType.Rocket,
        ["EMP"] = SubWeaponType.EMP,
        ["AntiAir"] = SubWeaponType.AntiAir,
        ["LaserWeapon"] = SubWeaponType.Ray
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

    public static SubWeaponType[] ToSubWeaponTypes(this string? tag, WeaponType weaponType)
    {
        var subTypes = weaponType switch
        {
            WeaponType.Close => _closeSubWeaponTypes,
            WeaponType.HangShoulder => _hangShoulderSubWeaponTypes,
            _ => _subWeaponTypes
        };

        return ("All|" + tag)
            .SplitStringOr()
            .Where(x => x is not "CantGen")
            .Select(type =>
            {
                if (!subTypes.TryGetValue(type, out var subType))
                {
                    throw new InvalidDataException($"Unknown weapon sub type: {type}");
                }

                return subType;
            })
            .Distinct()
            .OrderBy(x => x)
            .ToArray();
    }
}