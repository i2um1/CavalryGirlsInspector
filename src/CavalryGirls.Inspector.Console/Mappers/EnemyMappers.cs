using CavalryGirls.Inspector.Models;
using CavalryGirls.Inspector.Utils;

namespace CavalryGirls.Inspector.Mappers;

public static class EnemyMappers
{
    public static Enemy ToEnemy(int index, RawEnemy rawEnemy, Description description)
        => new()
        {
            Index = index,
            Id = rawEnemy.Id ?? throw new ArgumentNullException(rawEnemy.Id),
            Name = description.Name,
            Description = description.Value,
            Hp = rawEnemy.Hp,
            MovementSpeed = rawEnemy.MovementSpeed,
            RotationalSpeed = rawEnemy.RotationalSpeed,
            Armor = rawEnemy.Armor,
            ExplosiveResistance = rawEnemy.ExplosiveResistance,
            StatImmunity = rawEnemy.StatImmunity,
            StatResistance = rawEnemy.StatResistance,
            WeaponIds = rawEnemy.WeaponIds.SplitIntOr(),
            FieldOfVision = rawEnemy.FieldOfVision,
            IsFlying = rawEnemy.IsFlying,
            Drop = rawEnemy.Drop.SplitStringOr(),
            Parameters = rawEnemy.Parameters.SplitStringOr(),
            PreFireTime = rawEnemy.PreFireTime,
            IsBoss = rawEnemy.IsBoss,
            IsBuilding = rawEnemy.IsBuilding is "1"
        };
}