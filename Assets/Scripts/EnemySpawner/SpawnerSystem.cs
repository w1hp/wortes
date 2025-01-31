using Unity.Entities;
using Random = Unity.Mathematics.Random;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
    private EnemySpawnerComponent enemySpawnerComponent;
    private EnemyDataContainer enemyDataContainer;
    private Entity enemySpawnerEntity;
    private Entity characterEntity;
    private float nextSpawnTime;
    private Random random;

    private float spawnInterval = 2f; 
    private float spawnAccelerationRate = 0.95f; 
    private double elapsedTime;

    private const float minSpawnDistance = 25f; 
    private const float maxSpawnDistance = 50f;

    protected override void OnCreate()
    {
        base.OnCreate();
        random = Random.CreateFromIndex((uint)UnityEngine.Time.realtimeSinceStartup);
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out enemySpawnerEntity))
        {
            return;
        }

        if (!SystemAPI.TryGetSingletonEntity<CharacterAnimation>(out characterEntity))
        {
            return;
        }

        enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(enemySpawnerEntity);
        enemyDataContainer = EntityManager.GetComponentObject<EnemyDataContainer>(enemySpawnerEntity);

        // Check if enemy list is not empty
        if (enemyDataContainer.enemies == null || enemyDataContainer.enemies.Count == 0)
        {
            Debug.LogWarning("No enemies found in EnemyDataContainer!");
            return;
        }

        elapsedTime = SystemAPI.Time.ElapsedTime;

        if (elapsedTime > nextSpawnTime)
        {
            SpawnEnemy();
            AdjustSpawnInterval();
        }
    }

    private void SpawnEnemy()
    {
        // Determine which enemy type to spawn based on elapsed time
        int enemyTypeIndex = 0; // Default to first type

        if (elapsedTime >= 60)
        {
            enemyTypeIndex = 2; // Third type
        }
        else if (elapsedTime >= 30)
        {
            enemyTypeIndex = 1; // Second type
        }

        // Ensure the index is within the bounds of the enemies list
        if (enemyTypeIndex >= enemyDataContainer.enemies.Count)
        {
            Debug.LogWarning("Enemy type index out of bounds!");
            return;
        }

        var enemyPrefab = enemyDataContainer.enemies[enemyTypeIndex].prefab;

        float groundLevel = 1.0f; // Adjust enemy spawn height to the correct ground level
        float3 spawnPosition;
        int attempts = 0;
        const int maxAttempts = 10;

        do
        {
            float angle = random.NextFloat(0, 2 * math.PI);
            float distance = random.NextFloat(minSpawnDistance, maxSpawnDistance);
            float offsetX = math.cos(angle) * distance;
            float offsetZ = math.sin(angle) * distance;
            float3 playerPosition = GetPlayerPosition();
            spawnPosition = new float3(playerPosition.x + offsetX, groundLevel, playerPosition.z + offsetZ);
            attempts++;
        } while (attempts < maxAttempts);

        var instance = EntityManager.Instantiate(enemyPrefab);
        EntityManager.SetComponentData(instance, LocalTransform.FromPosition(spawnPosition));

        // Ensure enemy starts with a valid transform for AI tracking
        if (!EntityManager.HasComponent<LocalTransform>(instance))
        {
            EntityManager.AddComponentData(instance, LocalTransform.FromPosition(spawnPosition));
        }

        // Assign health and elemental properties
        var enemyData = enemyDataContainer.enemies[enemyTypeIndex];
        EntityManager.AddComponentData(instance, new Health
        {
            CurrentHealth = enemyData.maxHealth,
            FireResistance = enemyData.fireResistance,
            WaterResistance = enemyData.waterResistance,
            EarthResistance = enemyData.earthResistance,
            WoodResistance = enemyData.woodResistance,
            MetalResistance = enemyData.metalResistance
        });

        // Assign movement speed and detection range to EnemyComponent
        EntityManager.AddComponentData(instance, new EnemyComponent
        {
            DetectionRange = enemyData.detectionRange,
            Damage = enemyData.damage,
            moveSpeed = enemyData.moveSpeed,
            EnemyType = enemyData.elementType
        });

        nextSpawnTime = (float)elapsedTime + spawnInterval;
    }

    private void AdjustSpawnInterval()
    {
        // Gradually decrease spawn interval
        spawnInterval *= spawnAccelerationRate;
        spawnInterval = math.max(spawnInterval, 0.5f); // Minimum interval of 0.5 seconds
    }

    private float3 GetPlayerPosition()
    {
        if (EntityManager.HasComponent<LocalTransform>(characterEntity))
        {
            float3 playerPos = EntityManager.GetComponentData<LocalTransform>(characterEntity).Position;
            Debug.Log($"Player position: {playerPos}");
            return playerPos;
        }

        Debug.LogWarning("Character entity has no LocalTransform component!");
        return float3.zero; // Default position if player not found
    }
}