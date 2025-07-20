using CsvHelper.Configuration.Attributes;

namespace CavalryGirls.Inspector.Models;

public sealed class RawEnemy
{
    [Name("Enemys")] public string? Id { get; set; }
    [Name("Content")] public string? Content { get; set; }
    [Name("Name")] public string? Name { get; set; }
    [Name("Company")] public string? Company { get; set; }
    [Name("Description")] public string? Description { get; set; }
    [Name("MaxHp")] public int Hp { get; set; }
    [Name("MaxMoveSpeed")] public int MovementSpeed { get; set; }
    [Name("RotateSpeed")] public double RotationalSpeed { get; set; }
    [Name("Armor")] public int Armor { get; set; }
    [Name("ExploResist")] public int ExplosiveResistance { get; set; }
    [Name("Immune")] public int StatImmunity { get; set; }
    [Name("Resist")] public int StatResistance { get; set; }
    [Name("WeaponID")] public string? WeaponIds { get; set; }
    [Name("Range")] public int FieldOfVision { get; set; }
    [Name("Flying")] public bool IsFlying { get; set; }
    [Name("Price")] public double Price { get; set; }
    [Name("Price_Chan")] public double PriceChan { get; set; }
    [Name("DemoPrice")] public int DemoPrice { get; set; }
    [Name("UnitPeriodTime")] public string? UnitPeriodTime { get; set; }
    [Name("Drop")] public string? Drop { get; set; }
    [Name("InData")] public string? InData { get; set; }
    [Name("DontCount")] public string? DontCount { get; set; }
    [Name("Params")] public string? Parameters { get; set; }
    [Name("PreFireTime")] public double PreFireTime { get; set; }
    [Name("IsFocus")] public int IsFocus { get; set; }
    [Name("DieEffect")] public string? DieEffect { get; set; }
    [Name("AllowedDebug")] public bool IsAllowedDebug { get; set; }
    [Name("IsBoss")] public bool IsBoss { get; set; }
    [Name("IsBuilding")] public string? IsBuilding { get; set; }
    [Name("HalfHighAngleFloat")] public double HalfHighAngle { get; set; }
    [Name("HalfLowAngleFloat")] public double HalfLowAngle { get; set; }
}