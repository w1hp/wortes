using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class LevelUpSystem : SystemBase
{
	[SerializeField] private PowerUpSO powerUpSO_0;
	[SerializeField] private PowerUpSO powerUpSO_1;
	[SerializeField] private PowerUpSO powerUpSO_2;

	public event Action<PowerUpSO, PowerUpSO, PowerUpSO> LevelUp;

	protected override void OnCreate()
	{
		RequireForUpdate(SystemAPI.QueryBuilder().WithAll<CharacterComponent, Inventory>().Build());
	}

	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);
		foreach (var(characterComponent, inventory) in 
			SystemAPI.Query<RefRO<CharacterComponent>, RefRW<Inventory>>())
		{
			if (CanLevelUp(inventory.ValueRO.Gold, inventory.ValueRO.Level))
			{
				inventory.ValueRW.Level++;
				LevelUp?.Invoke(powerUpSO_0, powerUpSO_1, powerUpSO_2);


			}
		}
	}

	bool CanLevelUp(float gold, float level)
	{
		return gold >= 10 * math.pow(level,2);
	}
}