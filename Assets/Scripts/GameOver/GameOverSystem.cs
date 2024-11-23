using System;
using Unity.Burst;
using Unity.Entities;


[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class GameOverSystem : SystemBase
{
	public event Action<bool, float> OnGameOver;
	protected override void OnCreate()
	{
		RequireForUpdate<GameOverTag>();
	}
	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);
		
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		entityManager.WorldUnmanaged.GetExistingSystemState<EnemySpawnerSystem>().Enabled = true;

		var gameOverEntity = SystemAPI.GetSingletonEntity<GameOverTag>();
		var gameOverComponent = SystemAPI.GetComponent<GameOverTag>(gameOverEntity);


		var killStatistics = SystemAPI.GetSingleton<KillStatistics>();
		var countUpTime = SystemAPI.GetSingleton<CountUpTime>();
		int sceneIndex = StateUI.Singleton.LastSceneIndex;

		var playerEntity = SystemAPI.GetSingletonEntity<CharacterComponent>();
		var playerHealth = SystemAPI.GetComponent<Health>(playerEntity);
		var playerResources = SystemAPI.GetComponent<CharacterResources>(playerEntity);

		var towerBuiltCount = SystemAPI.GetSingleton<TowerBuiltCount>();
		OnGameOver?.Invoke(gameOverComponent.Success, playerResources.Gold); 

#if UNITY_EDITOR
		UnityEngine.Debug.Log($"Success: {gameOverComponent.Success}. " +
			$"Kill stats: all enemy({killStatistics.EnemyCount}), player({killStatistics.PlayerFragCount}), towers({killStatistics.TowerFragCount}). " +
			$"Index scene: {sceneIndex}. " +
			$"Player health: {playerHealth.CurrentHealth}, level: {playerResources.Level}. " +
			$"Elapsed time: {countUpTime.ElapsedTime}. " +
			$"Tower Built Count: {towerBuiltCount.Count}");
#endif

		ECB.AddComponent(gameOverEntity, new LevelEndedEventComponent
		{
			EnemyCount = killStatistics.EnemyCount,
			PlayerFragCount = killStatistics.PlayerFragCount,
			TowerFragCount = killStatistics.TowerFragCount,

			LevelID = sceneIndex,
			LevelSuccess = gameOverComponent.Success,

			PlayerHealth = (int)playerHealth.CurrentHealth,
			UserLevel = (int)playerResources.Level,

			Time = countUpTime.ElapsedTime,
			TowerCount = towerBuiltCount.Count,
		});

		ECB.RemoveComponent<GameOverTag>(gameOverEntity);
	}
}

partial struct GameOverTagSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (player, playerEntity) in
			SystemAPI.Query<PlayerTag>()
			.WithEntityAccess()
			.WithNone<IsExistTag>())
		{
#if UNITY_EDITOR
			UnityEngine.Debug.Log("Character is dead");
#endif
			var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
			var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

			ECB.AddComponent(playerEntity, new GameOverTag { Success = false });
			ECB.RemoveComponent<PlayerTag>(playerEntity);
		}

		foreach (var (boss, bossEntity) in SystemAPI.Query<BossTag>()
			.WithEntityAccess()
			.WithNone<IsExistTag>())
		{
#if UNITY_EDITOR
			UnityEngine.Debug.Log("Boss is dead");
#endif
			var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
			var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
			ECB.AddComponent(bossEntity, new GameOverTag { Success = true });
			ECB.RemoveComponent<BossTag>(bossEntity);
		}
	}
}
public struct GameOverTag : IComponentData
{
	public bool Success;
}