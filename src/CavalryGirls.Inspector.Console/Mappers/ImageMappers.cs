using CavalryGirls.Inspector.Models;

namespace CavalryGirls.Inspector.Mappers;

public static class ImageMappers
{
    public static string[] ToImagePaths(this Dictionary<int, Material> materials, string imagesFolder)
        => materials.Values.Select(x => ToImagePath(x.ImageFileName, imagesFolder)).ToArray();

    public static string[] ToImagePaths(this Dictionary<int, Fusion> fusions, string imagesFolder)
        => fusions.Values.Select(x => ToImagePath(x.ImageFileName, imagesFolder)).ToArray();

    public static string[] ToImagePaths(this Dictionary<int, WeaponModule> weaponModules, string imagesFolder)
        => weaponModules.Values.Select(x => ToImagePath(x.ImageFileName, imagesFolder)).ToArray();

    public static string[] ToImagePaths(this Dictionary<int, Weapon> weapons, string imagesFolder)
        => weapons.Values.Select(x => ToImagePath(x.ImageFileName, imagesFolder)).ToArray();

    public static string[] ToImagePaths(this Dictionary<string, Enemy> enemies, string imagesFolder)
        => enemies.Values.Select(x => ToImagePath(x.Id, imagesFolder)).ToArray();

    public static string ToImagePath(this string fileName, string imagesFolder)
        => Path.Combine(imagesFolder, fileName + ".png");
}