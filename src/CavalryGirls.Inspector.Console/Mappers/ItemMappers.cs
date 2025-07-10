using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Mappers;

public static class ItemMappers
{
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
            Functions = item.Functions.SplitFunctions(),
            Price = item.Price,
            Day = item.Day ?? throw new ArgumentNullException(item.Day),
            Level = item.Level,
            WeaponIds = item.Craft.SplitStringOr(),
            Ingredients = item.Ingredients.SplitCount()
        };
}