namespace CavalryGirls.Inspector.Models;

public sealed class WeaponModule
{
    public int Index { get; init; }
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required RawItem Raw { get; init; } // TODO
}