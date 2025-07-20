using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

using static CavalryGirls.Inspector.Mappers.EnemyMappers;

namespace CavalryGirls.Inspector.Repositories;

public sealed class RawEnemyRepository
{
    private const string FILE_NAME = "Enemys.txt";

    private readonly string _dataFolder;

    public RawEnemyRepository(string dataFolder)
    {
        _dataFolder = dataFolder;
    }

    public async Task<(Dictionary<string, Enemy> Enemies, HashSet<string> InvalidEnemies)> GetEnemies(
        Dictionary<string, Description> descriptions)
    {
        var rawEnemies = _dataFolder.ReadCsv<RawEnemy>(FILE_NAME);
        var result = new Dictionary<string, Enemy>();
        var invalidEnemies = new HashSet<string>();

        var index = 0;
        await foreach (var rawEnemy in rawEnemies)
        {
            if (string.IsNullOrEmpty(rawEnemy.Id))
            {
                continue;
            }

            if (!descriptions.TryGetValue(rawEnemy.Id, out var description))
            {
                invalidEnemies.Add(rawEnemy.Id);
                continue;
            }

            result[rawEnemy.Id] = ToEnemy(index, rawEnemy, description);
            index++;
        }

        return (result, invalidEnemies);
    }
}