using CavalryGirls.Inspector.Mappers;
using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Repositories;
using CavalryGirls.Inspector.Utils;

var path = "E:\\Projects\\1\\ExportedProject\\Assets\\Resources\\text\\table";

var rawDescriptionRepository = new RawDescriptionRepository(path);
var rawItemRepository = new RawItemRepository(path);
var rawBulletRepository = new RawBulletRepository(path);

var itemDescriptions = await rawDescriptionRepository.GetItemDescriptions();
var enemyDescriptions = await rawDescriptionRepository.GetEnemyDescriptions();

var materials = await rawItemRepository.GetMaterials(itemDescriptions);
var fusions = await rawItemRepository.GetFusions(itemDescriptions);
var weaponModules = await rawItemRepository.GetWeaponModules(itemDescriptions);
var weapons = await rawItemRepository.GetWeapons(itemDescriptions);

var bullets = await rawBulletRepository.GetBullets();

var weapon = weapons.Values.First(x => x.WeaponType is WeaponType.Weapon);

var imagesFolder = "e:\\Projects\\1\\ExportedProject\\Assets\\Resources\\texture\\property";
var atlas = await ImageAtlas.Create(weapons.ToImagePaths(imagesFolder), "result.webp");

Console.WriteLine();