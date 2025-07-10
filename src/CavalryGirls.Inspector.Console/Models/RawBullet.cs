using CsvHelper.Configuration.Attributes;

namespace CavalryGirls.Inspector.Models;

public sealed class RawBullet
{
    [Name("Bullets")] public string? Id { get; set; }
    [Name("Prefab")] public string? Prefab { get; set; }
    [Name("BulletRange")] public int BulletRange { get; set; }
    [Name("BulletSpeed")] public int BulletSpeed { get; set; }
    [Name("Damage")] public int Damage { get; set; }
    [Name("AntiAir")] public int AntiAir { get; set; }
    [Name("MaxHit")] public int MaxHit { get; set; }
    [Name("PiercingPossibility")] public int PiercingPossibility { get; set; }
    [Name("CriticDamage")] public int CriticDamage { get; set; }
    [Name("CriticPossibility")] public int CriticPossibility { get; set; }
    [Name("FireType")] public int FireType { get; set; }
    [Name("fireFlash")] public int FireFlash { get; set; }
    [Name("SetParent")] public int SetParent { get; set; }
    [Name("MinRange")] public int MinRange { get; set; }
    [Name("Debuff")] public int Debuff { get; set; }
    [Name("Kinetic")] public bool Kinetic { get; set; }
    [Name("SpecialParam")] public string? SpecialParam { get; set; }
}