using System;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class GameOverSystem : SystemBase
{
	public event Action<bool> OnGameOver;


	protected override void OnCreate()
	{
		RequireForUpdate<CharacterComponent>();
	}
	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);

		foreach (var (characterComponent, health, entity) in 
			SystemAPI.Query<RefRO<CharacterComponent>, RefRO<Health>>()
			.WithEntityAccess())
		{
			if (health.ValueRO.CurrentHealth <= 0f)
			{
				OnGameOver?.Invoke(true);
				ECB.DestroyEntity(entity);
			}
		}
	}
}
