namespace CavalryGirlsInspector.Console.Models;

// TODO
public sealed class Fusion(int index, RawItem item, Description description)
{
    public int Index { get; } = index;
    public int Id { get; } = item.Id;
    public string Name { get; } = description.Name;
    public string Description { get; } = description.Value;
    public Function[] Functions { get; } = GetFunctions(item.Functions);
    public int Price { get; } = item.Price;
    public string Day { get; } = item.Day ?? throw new ArgumentNullException(item.Day);
    public int Level { get; } = item.Level;
    public int[] Family { get; } = GetFamily(item.Family);
    public string[] BaseWeapons { get; } = GetBaseWeapons(item.Craft);
    public string[] FusionWeapons { get; } = GetFusionWeapons(item.Craft);

    public override string ToString() => $"{Id}: {Name}";

    private static int[] GetFamily(string? family)
    {
        ArgumentNullException.ThrowIfNull(family);

        return family
            .Split('|')
            .Select(x => int.Parse(x, CultureInfo.InvariantCulture))
            .ToArray();
    }

    private static Function[] GetFunctions(string? functions)
    {
        ArgumentNullException.ThrowIfNull(functions);

        return functions
            .Split('|')
            .Select(x =>
            {
                var parts = x.Split(':');
                return new Function(parts[0], parts[1]);
            })
            .ToArray();
    }

    private static string[] GetBaseWeapons(string? craft)
    {
        ArgumentNullException.ThrowIfNull(craft);

        return craft
            .Split('+')
            .First()
            .Split('|')
            .ToArray();
    }

    private static string[] GetFusionWeapons(string? craft)
    {
        ArgumentNullException.ThrowIfNull(craft);

        return craft
            .Split('+')
            .Last()
            .Split('|')
            .ToArray();
    }
}