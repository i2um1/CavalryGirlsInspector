using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Mappers;

public static class ItemMappers
{
    public static Bullet ToBullet(RawBullet rawBullet, RawWeapon rawWeapon)
        => new()
        {
            Id = rawWeapon.Id,
            Type = rawBullet.Id ?? throw new ArgumentNullException(rawBullet.Id),
            BulletRange = rawBullet.BulletRange,
            BulletSpeed = rawBullet.BulletSpeed,
            Damage = rawBullet.Damage,
            AntiAir = rawBullet.AntiAir,
            MaxHit = rawBullet.MaxHit,
            PiercingPossibility = rawBullet.PiercingPossibility,
            CriticDamage = rawBullet.CriticDamage,
            CriticPossibility = rawBullet.CriticPossibility,
            FireType = rawBullet.FireType,
            FireFlash = rawBullet.FireFlash,
            MinRange = rawBullet.MinRange,
            Debuff = rawBullet.Debuff,
            IsKinetic = rawBullet.IsKinetic,
            SpecialParameter = rawBullet.SpecialParameter
                               ?? throw new ArgumentNullException(rawBullet.SpecialParameter),
            FireRate = rawWeapon.FireRate,
            AngleFloat = rawWeapon.AngleFloat,
            FireNum = rawWeapon.FireNum,
            ReloadSpeed = rawWeapon.ReloadSpeed,
            Stable = rawWeapon.Stable,
            BrokePos = rawWeapon.BrokePos,
            BrokePos2 = rawWeapon.BrokePos2,
            CanReload = rawWeapon.Reload is "1",
            Force = rawWeapon.Force,
            Functions = rawWeapon.Functions.SplitFunctions(),
            DefaultModule = rawWeapon.DefaultModule.SplitIntOr(),
            DefaultFusion = rawWeapon.DefaultFusion.SplitIntOr()
        };

    public static Material ToMaterial(int index, RawItem item, Description description)
        => new()
        {
            Index = index,
            Id = item.Id,
            Name = description.Name,
            Description = description.Value,
            ImageFileName = item.ImageFileName ?? throw new ArgumentNullException(item.ImageFileName),
            Type = item.Functions ?? throw new ArgumentNullException(item.Functions),
            Price = item.Price,
            Day = item.Day ?? throw new ArgumentNullException(item.Day),
            Level = item.Level,
            StackSize = item.StackSize
        };

    public static Fusion ToFusion(int index, RawItem item, Description description)
    {
        var (baseCraft, fusionCraft) = item.Craft.SplitFusionCrafts();
        return new Fusion
        {
            Index = index,
            Id = item.Id,
            Name = description.Name,
            Description = description.Value,
            ImageFileName = item.ImageFileName ?? throw new ArgumentNullException(item.ImageFileName),
            Tag = item.Tag ?? throw new ArgumentNullException(item.Tag),
            WeaponType = item.Type.ConvertFusionToWeaponType(),
            Functions = item.Functions.SplitFunctions(),
            Price = item.Price,
            Day = item.Day ?? throw new ArgumentNullException(item.Day),
            Level = item.Level,
            Family = item.Family.SplitIntOr(),
            BaseWeapons = baseCraft.SplitStringOr(),
            FusionWeapons = fusionCraft.SplitStringOr()
        };
    }

    public static WeaponModule ToWeaponModule(int index, RawItem item, Description description)
        => new()
        {
            Index = index,
            Id = item.Id,
            Name = description.Name,
            Description = description.Value,
            ImageFileName = item.ImageFileName ?? throw new ArgumentNullException(item.ImageFileName),
            Tag = item.Tag ?? throw new ArgumentNullException(item.Tag),
            WeaponType = item.Type.ConvertModuleToWeaponType(),
            Functions = item.Functions.SplitFunctions(),
            Price = item.Price,
            Day = item.Day ?? throw new ArgumentNullException(item.Day),
            Level = item.Level,
            WeaponIds = item.Craft.SplitStringOr(),
            Materials = item.Materials.SplitCount()
        };

    public static Weapon ToWeapon(int index, RawItem item, Description description)
    {
        var weaponType = item.Tag is "RiotShiled" ? WeaponType.Close : item.Type.ConvertWeaponToWeaponType();
        var functions = item.Functions.SplitFunctions();

        var bulletFunction = functions.FirstOrDefault(x => x.Name is "Weapon" or "ArmorStrategy")
                             ?? throw new InvalidDataException($"Weapon {item.Id} does not have a bullet");

        return new Weapon
        {
            Index = index,
            Id = item.Id,
            Name = description.Name,
            Description = description.Value,
            ImageFileName = item.ImageFileName ?? throw new ArgumentNullException(item.ImageFileName),
            WeaponType = weaponType,
            WeaponSubTypes = item.Tag.ToSubWeaponTypes(weaponType),
            BulletId = bulletFunction.Value,
            Functions = functions,
            Price = item.Price,
            Day = item.Day ?? throw new ArgumentNullException(item.Day),
            Level = item.Level,
            Family = item.Family.SplitIntOr(),
            WeaponIds = item.Craft.SplitStringOr(),
            Materials = item.Materials.SplitCount()
        };
    }
}