using System;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class GameOverSystem : SystemBase
{
	public event Action<float> OnGameOver;


	protected override void OnCreate()
	{
		RequireForUpdate<CharacterComponent>();
		RequireForUpdate<IsNotPause>();
	}
	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);

		foreach (var (characterComponent, health, inventory, entity) in 
			SystemAPI.Query<RefRO<CharacterComponent>, RefRO<Health>, RefRO<Inventory>>()
			.WithEntityAccess())
		{
			if (health.ValueRO.CurrentHealth <= 0f)
			{
				OnGameOver?.Invoke(inventory.ValueRO.Gold);
				ECB.DestroyEntity(entity);
			}
		}
	}
}
