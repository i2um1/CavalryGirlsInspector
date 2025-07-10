using CavalryGirls.Inspector.Repositories;

var path = "E:\\Projects\\1\\ExportedProject\\Assets\\Resources\\text\\table";

var rawDescriptionRepository = new RawDescriptionRepository(path);
var rawItemRepository = new RawItemRepository(path);

var itemDescriptions = await rawDescriptionRepository.GetItemDescriptions();
var enemyDescriptions = await rawDescriptionRepository.GetEnemyDescriptions();

var fusions = await rawItemRepository.GetFusions(itemDescriptions);
var weaponModules = await rawItemRepository.GetWeaponModules(itemDescriptions);

var weaponModule1 = weaponModules.First(x => x.Value.Raw.Craft.Length > 0);
var weaponModule2 = weaponModules.First(x => x.Value.Raw.Ingredients.Length > 0);

var set = new HashSet<string>();
foreach (var fusion in fusions.Values)
{
    foreach (var function in fusion.Functions)
    {
        set.Add(function.Name);
    }
}

foreach (var item in set)
{
    Console.WriteLine(item);
}

var test = fusions[2538];

Console.WriteLine();