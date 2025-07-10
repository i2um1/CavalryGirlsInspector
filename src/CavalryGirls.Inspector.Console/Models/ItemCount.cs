namespace CavalryGirls.Inspector.Models;

public sealed record ItemCount(int Id, int Count)
{
    public override string ToString() => $"{Id}: {Count}";
}