using CavalryGirls.Inspector.Repositories;

var path = "E:\\Projects\\1\\ExportedProject\\Assets\\Resources\\text\\table";

var rawDescriptionRepository = new RawDescriptionRepository(path);
var rawItemRepository = new RawItemRepository(path);
var rawBulletRepository = new RawBulletRepository(path);

var itemDescriptions = await rawDescriptionRepository.GetItemDescriptions();
var enemyDescriptions = await rawDescriptionRepository.GetEnemyDescriptions();

var fusions = await rawItemRepository.GetFusions(itemDescriptions);
var weaponModules = await rawItemRepository.GetWeaponModules(itemDescriptions);

var rawBullets = await rawBulletRepository.GetBullets();

var functions = fusions.Values
    .SelectMany(x => x.Functions)
    .Concat(weaponModules.Values.SelectMany(x => x.Functions))
    .Select(x => x.Name)
    .Order()
    .ToHashSet();

foreach (var function in functions)
{
    Console.WriteLine(function);
}

Console.WriteLine();