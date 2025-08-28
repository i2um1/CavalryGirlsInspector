using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

using Spectre.Console;

using static CavalryGirls.Inspector.Mappers.ItemMappers;

namespace CavalryGirls.Inspector.Repositories;

public sealed class RawBulletRepository
{
    private const string BULLETS_FILE_NAME = "Bullets.txt";
    private const string WEAPONS_FILE_NAME = "Weapons.txt";
    private const string SHOULDER_WEAPON_FILE_NAME = "ShoulderWeapon.txt";

    private readonly string _dataFolder;

    public RawBulletRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<Dictionary<int, Bullet>> GetBullets()
    {
        var rawBullets = await GetRawBullets();
        var rawWeapons = await GetRawWeapons();
        var result = new Dictionary<int, Bullet>();

        foreach (var rawWeapon in rawWeapons)
        {
            ArgumentNullException.ThrowIfNull(rawWeapon.ProjectileId);

            if (rawBullets.TryGetValue(rawWeapon.ProjectileId, out var rawBullet))
            {
                result[rawWeapon.Id] = ToBullet(rawBullets[rawWeapon.ProjectileId], rawWeapon);
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]No bullet {rawWeapon.ProjectileId}[/]");
            }
        }

        var rawShoulderWeapons = await GetRawShoulderWeapons();
        foreach (var rawShoulderWeapon in rawShoulderWeapons)
        {
            result[rawShoulderWeapon.Id] = ToBullet(rawShoulderWeapon);
        }

        return result;
    }

    private async Task<Dictionary<string, RawBullet>> GetRawBullets()
    {
        var rawBullets = _dataFolder.ReadCsv<RawBullet>(BULLETS_FILE_NAME);
        var result = new Dictionary<string, RawBullet>();

        await foreach (var rawBullet in rawBullets)
        {
            if (rawBullet.Id is null or "null")
            {
                continue;
            }

            result[rawBullet.Id] = rawBullet;
        }

        return result;
    }

    private async Task<List<RawWeapon>> GetRawWeapons()
    {
        var rawWeapons = _dataFolder.ReadCsv<RawWeapon>(WEAPONS_FILE_NAME);
        var result = new List<RawWeapon>();

        await foreach (var rawWeapon in rawWeapons)
        {
            if (rawWeapon.Id < 0)
            {
                continue;
            }

            result.Add(rawWeapon);
        }

        return result;
    }

    private async Task<List<RawShoulderWeapon>> GetRawShoulderWeapons()
    {
        var rawShoulderWeapons = _dataFolder.ReadCsv<RawShoulderWeapon>(SHOULDER_WEAPON_FILE_NAME);
        var result = new List<RawShoulderWeapon>();

        await foreach (var rawShoulderWeapon in rawShoulderWeapons)
        {
            result.Add(rawShoulderWeapon);
        }

        return result;
    }
}