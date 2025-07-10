namespace CavalryGirls.Inspector.Models;

public sealed class Fusion
{
    public int Index { get; init; }
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Function[] Functions { get; init; }
    public int Price { get; init; }
    public required string Day { get; init; }
    public int Level { get; init; }
    public required int[] Family { get; init; }
    public required string[] BaseWeapons { get; init; }
    public required string[] FusionWeapons { get; init; }

    public override string ToString() => $"{Id}: {Name}";
}