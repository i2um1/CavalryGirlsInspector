namespace CavalryGirls.Inspector.Models;

public sealed record WeaponSubTypeCondition(ConditionType Type, WeaponSubType Value)
{
    public override string ToString() => $"{Type}: {Value}";
}