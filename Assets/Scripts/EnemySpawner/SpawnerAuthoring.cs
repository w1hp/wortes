using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Mathematics;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public float spawnCooldown = 1;
    public Vector3 cameraSize;
    public List<EnemySO> enemiesSO;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity enemySpawnerAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(enemySpawnerAuthoring, new EnemySpawnerComponent
            {
                spawnCooldown = authoring.spawnCooldown,
                cameraSize = authoring.cameraSize
            });

            List<EnemyData> enemyData = new List<EnemyData>();

            foreach (EnemySO e in authoring.enemiesSO)
            {
                enemyData.Add(new EnemyData
                {
                    level = e.level,
					prefab = GetEntity(e.prefab, TransformUsageFlags.None),
					detectionRange = e.detectionRange,
					elementType = e.elementType,
                    maxHealth = e.maxHealth,
					fireResistance = e.fireResistance,
					waterResistance = e.waterResistance,
					earthResistance = e.earthResistance,
					woodResistance = e.woodResistance,
					metalResistance = e.metalResistance,
					damage = e.damage,
                    moveSpeed = e.moveSpeed
                });
            }

            AddComponentObject(enemySpawnerAuthoring, new EnemyDataContainer { enemies = enemyData });
        }
    }
}
