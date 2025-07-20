namespace CavalryGirls.Inspector.Models;

public sealed class Enemy
{
    public int Index { get; set; }
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public int Hp { get; set; }
    public int MovementSpeed { get; set; }
    public double RotationalSpeed { get; set; }
    public int Armor { get; set; }
    public int ExplosiveResistance { get; set; }
    public int StatImmunity { get; set; }
    public int StatResistance { get; set; }
    public required int[] WeaponIds { get; set; }
    public int FieldOfVision { get; set; }
    public bool IsFlying { get; set; }
    public required string[] Drop { get; set; }
    public required string[] Parameters { get; set; }
    public double PreFireTime { get; set; }
    public bool IsBoss { get; set; }
    public bool IsBuilding { get; set; }

    public override string ToString() => $"{Id}: {Name}";
}