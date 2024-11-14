using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;



partial struct BossInitializationSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate(SystemAPI.QueryBuilder()
			.WithAll<BossInitialization>()
			.WithNone<BossCountdown>()
			.Build());
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (bossInitialization, entity) in SystemAPI.Query<BossInitialization>().WithEntityAccess())
		{
			//Disable enemy spawner
			var enemySpawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerComponent>();
			ECB.DestroyEntity(enemySpawnerEntity);

			//Kill all enemies
			var enemyQuery = SystemAPI.QueryBuilder()
				.WithAll<EnemyTag>()
				.WithNone<Prefab>().Build();
			var enemyEntities = enemyQuery.ToEntityArray(Allocator.Temp);
			foreach (var enemyEntity in enemyEntities)
			{
				ECB.SetComponentEnabled<IsExistTag>(enemyEntity, false);
			}
			//TASK: Diable player controls

			// Spawn boss
			Entity bossEntity = state.EntityManager.Instantiate(bossInitialization.BossPrefabEntity);

			//Set boss position
			var playerQuery = SystemAPI.QueryBuilder()
				.WithAll<PlayerTag>()
				.WithNone<Prefab>().Build();
			var playerEntity = playerQuery.ToEntityArray(Allocator.Temp);
			var playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity[0]);
			playerTransform.Position.z += 7;
			ECB.SetComponent(bossEntity, playerTransform);

			ECB.RemoveComponent<BossInitialization>(entity);
			//TASK: Enable player controls
		}
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{

	}
}
