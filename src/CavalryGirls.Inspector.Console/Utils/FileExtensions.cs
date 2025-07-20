using CsvHelper;

namespace CavalryGirls.Inspector.Utils;

public static class FileExtensions
{
    private const string ASSETS_OUTPUT_FOLDER = "assets";

    private const string TABLE_FOLDER = "Resources/text/table";
    private const string PROPERTY_FOLDER = "Resources/texture/property";
    private const string ENEMY_FOLDER = "Resources/texture/enemypre";

    private static readonly JsonSerializerOptions _jsonOptions;

    static FileExtensions()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public static string GetAssetsOutputFolder(this string? basePath)
    {
        ArgumentNullException.ThrowIfNull(basePath);
        return Path.Combine(basePath, ASSETS_OUTPUT_FOLDER);
    }

    public static string GetTableFolder(this string? basePath)
    {
        ArgumentNullException.ThrowIfNull(basePath);
        return Path.Combine(basePath, TABLE_FOLDER);
    }

    public static string GetPropertyFolder(this string? basePath)
    {
        ArgumentNullException.ThrowIfNull(basePath);
        return Path.Combine(basePath, PROPERTY_FOLDER);
    }

    public static string GetEnemyFolder(this string? basePath)
    {
        ArgumentNullException.ThrowIfNull(basePath);
        return Path.Combine(basePath, ENEMY_FOLDER);
    }

    public static async IAsyncEnumerable<T> ReadCsv<T>(
        this string folderPath,
        string fileName,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(folderPath, fileName);
        var content = await File.ReadAllTextAsync(path, cancellationToken);
        content = content.Replace('"', '\'');

        using var reader = new StringReader(content);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecordsAsync<T>(cancellationToken);
        await foreach (var record in records)
        {
            yield return record;
        }
    }

    public static async Task SaveJson<T>(this T data, string folderPath, string fileName)
    {
        folderPath.CreateFolderIfRequired();

        var path = Path.Combine(folderPath, fileName);
        var json = JsonSerializer.Serialize(data, _jsonOptions);

        await File.WriteAllTextAsync(path, json);
    }

    public static void CreateFolderIfRequired(this string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }
}