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
#if UNITY_EDITOR
			UnityEngine.Debug.Log("Boss is dead");
#endif
			entityManager.WorldUnmanaged.GetExistingSystemState<EnemySpawnerSystem>().Enabled = true;

			var characterResources = SystemAPI.GetSingleton<CharacterResources>();

			OnGameOver?.Invoke(true, characterResources.Gold);
			ECB.DestroyEntity(bossEntity);
		}

	}
}
