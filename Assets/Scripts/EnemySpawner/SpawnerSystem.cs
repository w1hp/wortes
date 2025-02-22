using Unity.Entities;
using Random = Unity.Mathematics.Random;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System;

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
        // Na podstawie czasu ustalamy, ile przeciwników ma się pojawić
        int numberOfEnemiesToSpawn = CalculateEnemiesToSpawn(elapsedTime);
        int currentEnemyTypeIndex = 0;
		for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
			int maxEnemyTypeIndex = enemySpawnerComponent.level - 1;
            int enemyTypeIndex = currentEnemyTypeIndex;
			//int enemyTypeIndex = (elapsedTime >= 60) ? 2 : (elapsedTime >= 30) ? 1 : 0;
			if (enemyTypeIndex >= enemyDataContainer.enemies.Count)
            {
                Debug.LogWarning("Enemy type index out of bounds!");
                return;
            }
            //Debug.Log("Spawning enemy type: " + enemyTypeIndex);
			var enemyPrefab = enemyDataContainer.enemies[enemyTypeIndex].prefab;
            float groundLevel = 1.0f;
            float3 spawnPosition;

            float angle = random.NextFloat(0, 2 * math.PI);
            float distance = random.NextFloat(minSpawnDistance, maxSpawnDistance);
            float offsetX = math.cos(angle) * distance;
            float offsetZ = math.sin(angle) * distance;
            float3 playerPosition = GetPlayerPosition();
            spawnPosition = new float3(playerPosition.x + offsetX, groundLevel, playerPosition.z + offsetZ);

            // Instantiowanie przeciwnika
            var instance = EntityManager.Instantiate(enemyPrefab);
            EntityManager.SetComponentData(instance, LocalTransform.FromPosition(spawnPosition));

            // Dodanie komponentów do przeciwnika
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
			EntityManager.AddComponentData(instance, new IsExistTag { });

			EntityManager.AddComponentData(instance, new EnemyTag { });

			EntityManager.AddComponentData(instance, new DamageableTag { });

			EntityManager.AddComponentData(instance, new CharacterExperiencePoints
			{
				Value = enemyData.experiencePoints
			});

			EntityManager.AddComponentData(instance, new Drop
			{
				ResourcePrefab = enemyData.resourcePrefab,
				ResourceAmount = enemyData.resourceAmount
			});

			EntityManager.AddComponentData(instance, new EnemyComponent
            {
                DetectionRange = enemyData.detectionRange,
                Damage = enemyData.damage,
                moveSpeed = enemyData.moveSpeed,
                EnemyType = enemyData.elementType
            });
			currentEnemyTypeIndex++;
			if (currentEnemyTypeIndex > maxEnemyTypeIndex)
			{
				currentEnemyTypeIndex = 0;
			}
		}

        // Zaktualizowanie czasu kolejnego spawnu
        nextSpawnTime = (float)elapsedTime + spawnInterval;
    }

    //private int CalculateEnemiesToSpawn(double elapsedTime)
    //{
    //    // Funkcja wykładnicza rosnąca z czasem
    //    float spawnFactor = (float)(Math.Exp(0.03 * elapsedTime) - 1); // Możesz dostosować wartość 0.03
    //    int numberOfEnemies = Mathf.FloorToInt(spawnFactor);

    //    // Upewnij się, że zawsze spawnujemy co najmniej 1 przeciwnika
    //    return Mathf.Max(1, numberOfEnemies);
    //}

    private int CalculateEnemiesToSpawn(double elapsedTime)
    {
        // Funkcja logarytmiczna rosnąca z czasem, ale wolniej niż wykładnicza
        float spawnFactor = Mathf.Log((float)(elapsedTime + 1)); // Dodajemy 1, żeby uniknąć log(0)
        int numberOfEnemies = Mathf.FloorToInt(spawnFactor);

        // Upewnij się, że zawsze spawnujemy co najmniej 1 przeciwnika
        return Mathf.Max(1, numberOfEnemies);
    }


    private void AdjustSpawnInterval()
    {
        // Gradual decrease in spawn interval
        spawnInterval *= spawnAccelerationRate;
        spawnInterval = math.max(spawnInterval, 0.7f); // Minimum interval of 0.5 seconds
    }

    private float3 GetPlayerPosition()
    {
        if (EntityManager.HasComponent<LocalTransform>(characterEntity))
        {
            float3 playerPos = EntityManager.GetComponentData<LocalTransform>(characterEntity).Position;
            return playerPos;
        }

        return float3.zero; // Default position if player not found
    }
}
