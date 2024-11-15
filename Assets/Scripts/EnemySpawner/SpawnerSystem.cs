using Unity.Entities;
using Random = Unity.Mathematics.Random;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
	private EnemySpawnerComponent enemySpawnerComponent;
	private EnemyDataContainer enemyDataContainerComponent;
	private Entity enemySpawnerEntity;
	private float nextSpawnTime;
	private Random random;

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

		enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(enemySpawnerEntity);
		enemyDataContainerComponent = EntityManager.GetComponentObject<EnemyDataContainer>(enemySpawnerEntity);

		if (SystemAPI.Time.ElapsedTime > nextSpawnTime)
		{
			SpawnEnemy();
		}
	}

	private void SpawnEnemy()
	{
		int level = 3;
		List<EnemyData> availableEnemies = new List<EnemyData>();

		foreach (EnemyData enemyData in enemyDataContainerComponent.enemies)
		{
			if (enemyData.level <= level)
			{
				availableEnemies.Add(enemyData);
			}
		}

		int index = random.NextInt(availableEnemies.Count);

		Entity newEnemy = EntityManager.Instantiate(availableEnemies[index].prefab);
		EntityManager.SetComponentData(newEnemy, new LocalTransform
		{
			Position = GetPositionOutsideOfCameraRange(),
			Rotation = quaternion.identity,
			Scale = 1
		});

		EntityManager.AddComponentData(newEnemy, new EnemyComponent
		{
			DetectionRange = availableEnemies[index].detectionRange,
			EnemyType = availableEnemies[index].elementType,
			Damage = availableEnemies[index].damage,
		});

		EntityManager.AddComponentData(newEnemy, new IsExistTag { });

		EntityManager.AddComponentData(newEnemy, new EnemyTag { });

		EntityManager.AddComponentData(newEnemy, new Health
		{
			CurrentHealth = availableEnemies[index].maxHealth,
			FireResistance = availableEnemies[index].fireResistance,
			WaterResistance = availableEnemies[index].waterResistance,
			EarthResistance = availableEnemies[index].earthResistance,
			WoodResistance = availableEnemies[index].woodResistance,
			MetalResistance = availableEnemies[index].metalResistance
		});

		EntityManager.AddComponentData(newEnemy, new CharacterExperiencePoints
		{
			Value = availableEnemies[index].experiencePoints
		});

		EntityManager.AddComponentData(newEnemy, new Drop
		{
			ResourcePrefab = availableEnemies[index].resourcePrefab,
			ResourceAmount = availableEnemies[index].resourceAmount
		});

		nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + enemySpawnerComponent.spawnCooldown;
	}

	private float3 GetPositionOutsideOfCameraRange()
	{
		float3 position;
		do
		{
			position = new float3(
				random.NextFloat(-enemySpawnerComponent.cameraSize.x * 2, enemySpawnerComponent.cameraSize.x * 2),
				1,
				random.NextFloat(-enemySpawnerComponent.cameraSize.z * 2, enemySpawnerComponent.cameraSize.z * 2)
			);
		} while (math.abs(position.x) < enemySpawnerComponent.cameraSize.x && math.abs(position.z) < enemySpawnerComponent.cameraSize.z);

		Vector3 cameraPosition = Camera.main.transform.position;
		position.x += cameraPosition.x;
		position.z += cameraPosition.z;

		return position;
	}
}
