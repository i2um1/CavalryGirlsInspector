using CsvHelper.Configuration.Attributes;

namespace CavalryGirls.Inspector.Models;

public sealed class RawBullet
{
    [Name("Bullets")] public string? Id { get; set; }
    [Name("Prefab")] public string? Prefab { get; set; }
    [Name("BulletRange")] public string? BulletRange { get; set; }
    [Name("BulletSpeed")] public string? BulletSpeed { get; set; }
    [Name("Damage")] public string? Damage { get; set; }
    [Name("AntiAir")] public string? AntiAir { get; set; }
    [Name("MaxHit")] public string? MaxHit { get; set; }
    [Name("PiercingPossibility")] public string? PiercingPossibility { get; set; }
    [Name("CriticDamage")] public string? CriticDamage { get; set; }
    [Name("CriticPossibility")] public string? CriticPossibility { get; set; }
    [Name("FireType")] public string? FireType { get; set; }
    [Name("fireFlash")] public string? FireFlash { get; set; }
    [Name("SetParent")] public string? SetParent { get; set; }
    [Name("MinRange")] public string? MinRange { get; set; }
    [Name("Debuff")] public string? Debuff { get; set; }
    [Name("Kinetic")] public string? IsKinetic { get; set; }
    [Name("SpecialParam")] public string? SpecialParameter { get; set; }
}