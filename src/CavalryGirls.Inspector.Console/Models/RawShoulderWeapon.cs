using CsvHelper.Configuration.Attributes;

namespace CavalryGirls.Inspector.Models;

public sealed class RawShoulderWeapon
{
    [Name("ShoulderWeapon")] public int Id { get; set; }
    [Name("DefaultModule")] public string? DefaultModule { get; set; }
    [Name("DefaultFusion")] public string? DefaultFusion { get; set; }
}