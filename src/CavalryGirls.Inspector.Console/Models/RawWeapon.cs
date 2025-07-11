using CsvHelper.Configuration.Attributes;

namespace CavalryGirls.Inspector.Models;

public sealed class RawWeapon
{
    [Name("Weapons")] public int Id { get; set; }
    [Name("Projectile")] public string? ProjectileId { get; set; }
    [Name("Image")] public string? ImageFileName { get; set; }
    [Name("Sound")] public string? Sound { get; set; }
    [Name("ClipSound")] public string? ClipSound { get; set; }
    [Name("FireRate")] public double FireRate { get; set; }
    [Name("AngleFloat")] public int AngleFloat { get; set; }
    [Name("FireNum")] public int FireNum { get; set; }
    [Name("ClipNum")] public int ClipNum { get; set; }
    [Name("ReloadSpeed")] public double ReloadSpeed { get; set; }
    [Name("Stable")] public double Stable { get; set; }
    [Name("BrokePos")] public int BrokePos { get; set; }
    [Name("BrokePos2")] public int BrokePos2 { get; set; }
    [Name("Reload")] public string? Reload { get; set; }
    [Name("Case")] public string? Case { get; set; }
    [Name("Force")] public int Force { get; set; }
    [Name("Func")] public string? Functions { get; set; }
    [Name("DefaultModule")] public string? DefaultModule { get; set; }
    [Name("DefaultFusion")] public string? DefaultFusion { get; set; }
}