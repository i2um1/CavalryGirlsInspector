using CsvHelper;

namespace CavalryGirlsInspector.Console.Utils;

public static class FileExtensions
{
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
}