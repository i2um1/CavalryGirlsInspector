namespace CavalryGirls.Inspector.Models;

public sealed record Description(string Name, string Value)
{
    public override string ToString() => $"{Name}: {Value}";
}