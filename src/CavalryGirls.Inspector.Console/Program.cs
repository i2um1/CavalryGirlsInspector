using CavalryGirlsInspector.Console.Repositories;

var path = "E:\\Projects\\1\\ExportedProject\\Assets\\Resources\\text\\table";

var rawDescriptionRepository = new RawDescriptionRepository(path);
var rawItemRepository = new RawItemRepository(path);

var itemDescriptions = await rawDescriptionRepository.GetItemDescriptions();
var enemyDescriptions = await rawDescriptionRepository.GetEnemyDescriptions();

var fusions = await rawItemRepository.GetFusions(itemDescriptions);

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