using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Repositories;

public sealed class RawBulletRepository
{
    private const string FILE_NAME = "Bullets.txt";

    private readonly string _dataFolder;

    public RawBulletRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<Dictionary<string, RawBullet>> GetBullets()
    {
        var rawBullets = _dataFolder.ReadCsv<RawBullet>(FILE_NAME);
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
}