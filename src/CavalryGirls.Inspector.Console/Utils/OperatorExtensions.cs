using CavalryGirls.Inspector.Models;

namespace CavalryGirls.Inspector.Utils;

public static class OperatorExtensions
{
    public static string[] SplitStringOr(this string? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
    }

    public static int[] SplitIntOr(this string? value)
        => value.SplitStringOr().Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.ToInt()).ToArray();

    public static ItemCount[] SplitCount(this string? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value
            .SplitStringOr()
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x =>
            {
                var parts = x.Split('_');
                if (parts.Length is not 2)
                {
                    throw new InvalidOperationException("Invalid item count");
                }

                return new ItemCount(parts[0].ToInt(), parts[1].ToInt());
            })
            .ToArray();
    }

    public static Function[] SplitFunctions(this string? functions)
    {
        ArgumentNullException.ThrowIfNull(functions);

        return functions
            .SplitStringOr()
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x =>
            {
                if (!x.Contains(':'))
                {
                    return new Function(x, string.Empty);
                }

                var parts = x.Split(':');
                if (parts.Length is not 2)
                {
                    throw new InvalidOperationException("Invalid function");
                }

                return new Function(parts[0], parts[1]);
            })
            .ToArray();
    }

    public static (string Base, string Fusion) SplitFusionCrafts(this string? craft)
    {
        ArgumentNullException.ThrowIfNull(craft);

        if (craft == string.Empty)
        {
            return (string.Empty, string.Empty);
        }

        var parts = craft.Split('+');
        if (parts.Length is not 2)
        {
            throw new InvalidOperationException("Invalid fusion count");
        }

        return (parts[0], parts[1]);
    }

    private static int ToInt(this string value)
        => int.Parse(value, CultureInfo.InvariantCulture);
}