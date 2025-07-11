using CavalryGirls.Inspector.Repositories;

var path = "E:\\Projects\\1\\ExportedProject\\Assets\\Resources\\text\\table";

var rawDescriptionRepository = new RawDescriptionRepository(path);
var rawItemRepository = new RawItemRepository(path);
var rawBulletRepository = new RawBulletRepository(path);

var itemDescriptions = await rawDescriptionRepository.GetItemDescriptions();
var enemyDescriptions = await rawDescriptionRepository.GetEnemyDescriptions();

var materials = await rawItemRepository.GetMaterials(itemDescriptions);
var fusions = await rawItemRepository.GetFusions(itemDescriptions);
var weaponModules = await rawItemRepository.GetWeaponModules(itemDescriptions);

var bullets = await rawBulletRepository.GetBullets();

Console.WriteLine();

/*
Weapon

CloseWeapon
CloseWeaponModel
CloseWeaponDemo

HangShoulder
HangShoulderModel
HangShoulderDemo
*/

/*
NormalWeapon - Kinetic Weapon
!Riffle
Riffle - Sniper Weapon
MG - Machine Gun
All
!ShotWeapon
Rail - Guide Rail
ExploWeapon - Explosive Weapon
FlakeWeapon - Plasma Weapon
ShotWeapon - Spreadshot Weapon
SplitWeapon - Spraying Weapon
!MG
LaserWeapon - Ray Weapon
ElectricWeapon - Arc Weapon
MagWeapon  - Magnetoelectric Weapon
*/