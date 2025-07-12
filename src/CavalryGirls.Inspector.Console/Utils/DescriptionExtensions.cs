namespace CavalryGirls.Inspector.Utils;

public static class DescriptionExtensions
{
    private static readonly Regex _trimRegex =
        new(@"^(?:\\n|\s*)*|(?:\\n|\s*)*$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

    private static readonly Regex _parameterRegex =
        new(@"<Description:?(\d+)?>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

    public static string FixDescription(this string description)
    {
        var result = _parameterRegex.Replace(description, match =>
        {
            if (!match.Success)
            {
                return string.Empty;
            }

            int.TryParse(match.Groups[1].Value, CultureInfo.InvariantCulture, out var index);
            return $"<D{index}>";
        });

        return _trimRegex.Replace(result, _ => string.Empty);
    }
}