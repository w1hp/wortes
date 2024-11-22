using Unity.Burst;
using Unity.Collections;
using System.Collections.Generic;
using Unity.Entities;
//using UnityEngine.Analytics;
using Unity.Services.Analytics;

public partial struct LevelCompletedAnalyticsSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<LevelCompletedEventComponent>();
	}

	public void OnUpdate(ref SystemState state)
	{
		var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

		foreach (var (eventData, entity) in SystemAPI.Query<RefRO<LevelCompletedEventComponent>>().WithEntityAccess())
		{
			AnalyticsService.Instance.RecordEvent(new LevelCompletedEvent
			{
				EnemyCount = eventData.ValueRO.EnemyCount,
				Level_ID = eventData.ValueRO.LevelID,
				PlayerFragCount = eventData.ValueRO.PlayerFragCount,
				PlayerHealth = eventData.ValueRO.PlayerHealth,
				Time = eventData.ValueRO.Time,
				TowerCount = eventData.ValueRO.TowerCount,
				TowerFragCount = eventData.ValueRO.TowerFragCount,
				UserLevel = eventData.ValueRO.UserLevel
			});

			ecb.RemoveComponent<LevelCompletedEventComponent>(entity);
		}

		ecb.Playback(state.EntityManager);
		ecb.Dispose();
	}
}


public struct LevelCompletedEventComponent : IComponentData
{
	public int EnemyCount;
	public int LevelID;
	public int PlayerFragCount;
	public int PlayerHealth;
	public float Time; // Czas w sekundach
	public int TowerCount;
	public int TowerFragCount;
	public int UserLevel;
}

