using System;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class GameOverSystem : SystemBase
{
	public event Action<bool, float> OnGameOver;


	protected override void OnCreate()
	{
		//RequireForUpdate<CharacterComponent>();
	}
	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);

		foreach (var (characterComponent, characterEntity) in
			SystemAPI.Query<RefRO<CharacterComponent>>()
			.WithEntityAccess()
			.WithNone<IsExistTag>())
		{
#if UNITY_EDITOR
			UnityEngine.Debug.Log("Character is dead");
#endif
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
			var characterResources = SystemAPI.GetSingleton<CharacterResources>();

			OnGameOver?.Invoke(true, characterResources.Gold);
			ECB.DestroyEntity(bossEntity);
		}
	}
}
