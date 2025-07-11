namespace CavalryGirls.Inspector.Models;

public sealed class Material
{
    public int Index { get; init; }
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string ImageFileName { get; set; }
    public required string Type { get; init; }
    public int Price { get; init; }
    public required string Day { get; init; }
    public int Level { get; init; }
    public int StackSize { get; init; }

    public override string ToString() => $"{Id}: {Name}";
}

//40001,#废铁,40001,Material,#合成材料,,99,All,,1,999,1,0,0,0,0,,,,<color=#878787>|</color>,1133,