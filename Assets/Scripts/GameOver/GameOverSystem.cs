using System;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class GameOverSystem : SystemBase
{
	public event Action<bool, float> OnGameOver;
	private EntityManager entityManager;


	protected override void OnCreate()
	{
		//RequireForUpdate<CharacterComponent>();
	}
	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


		foreach (var (characterComponent, characterEntity) in
			SystemAPI.Query<RefRO<CharacterComponent>>()
			.WithEntityAccess()
			.WithNone<IsExistTag>())
		{
#if UNITY_EDITOR
			UnityEngine.Debug.Log("Character is dead");
#endif
			//SystemAPI.ManagedAPI.GetSingleton<EnemySpawnerSystem>().Enabled = true;
			entityManager.WorldUnmanaged.GetExistingSystemState<EnemySpawnerSystem>().Enabled = true;
			var characterResources = SystemAPI.GetComponent<CharacterResources>(characterEntity);

			OnGameOver?.Invoke(false, characterResources.Gold);
			ECB.DestroyEntity(characterEntity);

		}
		foreach (var (boss, bossEntity) in SystemAPI.Query<BossTag>()
			.WithEntityAccess()
			.WithNone<IsExistTag>())
		{
			var killStatistics = SystemAPI.GetSingleton<KillStatistics>();
			var countUpTime = SystemAPI.GetSingleton<CountUpTime>();
			int sceneIndex = StateUI.Singleton.LastSceneIndex;

			var player = SystemAPI.GetSingletonEntity<PlayerTag>();
			var playerHealth = SystemAPI.GetComponent<Health>(player);
			var playerResources = SystemAPI.GetComponent<CharacterResources>(player);

			var towerBuiltCount = SystemAPI.GetSingleton<TowerBuiltCount>();


#if UNITY_EDITOR
			UnityEngine.Debug.Log($"Boss is dead. " +
				$"Kill stats: all enemy({killStatistics.EnemyCount}), player({killStatistics.PlayerFragCount}), towers({killStatistics.TowerFragCount}). " +
				$"Index scene: {sceneIndex}. " +
				$"Player health: {playerHealth.CurrentHealth}, level: {playerResources.Level}. " +
				$"Elapsed time: {countUpTime.ElapsedTime}. " +
				$"Tower Built Count: {towerBuiltCount.Count}");
#endif

			ECB.AddComponent(bossEntity, new LevelCompletedEventComponent
			{
				EnemyCount = killStatistics.EnemyCount, 
				PlayerFragCount = killStatistics.PlayerFragCount, 
				TowerFragCount = killStatistics.TowerFragCount,
				
				LevelID = sceneIndex,

				PlayerHealth = (int)playerHealth.CurrentHealth, 
				UserLevel = (int)playerResources.Level,

				Time = countUpTime.ElapsedTime,
				TowerCount = towerBuiltCount.Count,
			});
			entityManager.WorldUnmanaged.GetExistingSystemState<EnemySpawnerSystem>().Enabled = true;

			var characterResources = SystemAPI.GetSingleton<CharacterResources>();

			OnGameOver?.Invoke(true, characterResources.Gold);
			ECB.RemoveComponent<BossTag>(bossEntity);
			//ECB.DestroyEntity(bossEntity);
		}

	}
}