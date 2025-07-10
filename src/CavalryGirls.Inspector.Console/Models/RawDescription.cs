using CsvHelper.Configuration.Attributes;

namespace CavalryGirlsInspector.Console.Models;

public sealed class RawDescription
{
    [Name("Descriptions")] public string? Id { get; set; }
    [Name("Chinese")] public string? Chinese { get; set; }
    [Name("ChineseTraditional")] public string? ChineseTraditional { get; set; }
    [Name("English")] public string? English { get; set; }
    [Name("Japanese")] public string? Japanese { get; set; }
}