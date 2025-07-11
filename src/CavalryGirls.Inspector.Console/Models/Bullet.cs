namespace CavalryGirls.Inspector.Models;

public sealed class Bullet
{
    public int Id { get; init; }
    public required string Type { get; init; }

    public int BulletRange { get; init; }
    public int BulletSpeed { get; init; }
    public int Damage { get; init; }
    public int AntiAir { get; init; }
    public int MaxHit { get; init; }
    public int PiercingPossibility { get; init; }
    public int CriticDamage { get; init; }
    public int CriticPossibility { get; init; }
    public int FireType { get; init; }
    public int FireFlash { get; init; }
    public int MinRange { get; init; }
    public int Debuff { get; init; }
    public bool IsKinetic { get; init; }
    public required string SpecialParameter { get; init; }

    public double FireRate { get; init; }
    public int AngleFloat { get; init; }
    public int FireNum { get; init; }
    public double ReloadSpeed { get; init; }
    public double Stable { get; init; }
    public int BrokePos { get; init; }
    public int BrokePos2 { get; init; }
    public bool CanReload { get; init; }
    public int Force { get; init; }
    public required Function[] Functions { get; init; }
    public required int[] DefaultModule { get; init; }
    public required int[] DefaultFusion { get; init; }

    public override string ToString() => $"{Id}: {Type}";
}