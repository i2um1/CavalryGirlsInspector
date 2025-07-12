namespace CavalryGirls.Inspector.Models;

public sealed class WeaponModule
{
    public int Index { get; init; }
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    [JsonIgnore] public string ImageFileName { get; init; } = string.Empty;
    public WeaponType WeaponType { get; init; }
    public required WeaponSubType[] WeaponSubTypes { get; init; }
    public required Function[] Functions { get; init; }
    public int Price { get; init; }
    public required string Day { get; init; }
    public int Level { get; init; }
    public required string[] ModulesIds { get; init; }
    public required ItemCount[] Materials { get; init; }

    public override string ToString() => $"{Id}: {Name}";
}