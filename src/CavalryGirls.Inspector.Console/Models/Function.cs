namespace CavalryGirlsInspector.Console.Models;

public sealed record Function(string Name, string Value)
{
    public override string ToString() => $"{Name}: {Value}";
}