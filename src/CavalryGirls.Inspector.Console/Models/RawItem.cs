using CsvHelper.Configuration.Attributes;

namespace CavalryGirls.Inspector.Models;

public sealed class RawItem
{
    [Name("Items")] public int Id { get; set; }
    [Name("Name")] public string? Name { get; set; }
    [Name("Image")] public string? ImageFileName { get; set; }
    [Name("Type")] public string? Type { get; set; }
    [Name("Description")] public string? Description { get; set; }
    [Name("Tag")] public string? Tag { get; set; }
    [Name("MaxPile")] public int StackSize { get; set; }
    [Name("Func")] public string? Functions { get; set; }
    [Name("Company")] public string? Company { get; set; }
    [Name("Price")] public int Price { get; set; }
    [Name("Day")] public string? Day { get; set; }
    [Name("Level")] public int Level { get; set; }
    [Name("CargoPiles")] public int CargoPiles { get; set; }
    [Name("CargoPilesMax")] public int CargoPilesMax { get; set; }
    [Name("CargoPilesFusion")] public int CargoPilesFusion { get; set; }
    [Name("CargoPilesFusionMax")] public int CargoPilesFusionMax { get; set; }
    [Name("Cluster")] public string? Family { get; set; }
    [Name("Syns")] public string? Materials { get; set; }
    [Name("NeedWeapon")] public string? Craft { get; set; }
    [Name("ColorPlus")] public string? ColorPlus { get; set; }
    [Name("ResolveID")] public int ResolveId { get; set; }
    [Name("DebugHide")] public bool? DebugHide { get; set; }
}