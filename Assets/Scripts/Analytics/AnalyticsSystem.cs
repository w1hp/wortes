using Unity.Collections;
using Unity.Entities;
using Unity.Services.Analytics;

public partial struct LevelCompletedAnalyticsSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<LevelEndedEventComponent>();
	}

	public void OnUpdate(ref SystemState state)
	{
		var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

		foreach (var (eventData, entity) in SystemAPI.Query<RefRO<LevelEndedEventComponent>>().WithEntityAccess())
		{
			AnalyticsService.Instance.RecordEvent(new LevelEndedEvent
			{
				EnemyCount = eventData.ValueRO.EnemyCount,
				Level_ID = eventData.ValueRO.LevelID,
				LevelSuccess = eventData.ValueRO.LevelSuccess,
				PlayerFragCount = eventData.ValueRO.PlayerFragCount,
				PlayerHealth = eventData.ValueRO.PlayerHealth,
				Time = eventData.ValueRO.Time,
				TowerCount = eventData.ValueRO.TowerCount,
				TowerFragCount = eventData.ValueRO.TowerFragCount,
				UserLevel = eventData.ValueRO.UserLevel
			});

			ecb.RemoveComponent<LevelEndedEventComponent>(entity);
		}

		ecb.Playback(state.EntityManager);
		ecb.Dispose();
	}
}

public struct LevelEndedEventComponent : IComponentData
{
	public int EnemyCount;
	public int LevelID;
	public bool LevelSuccess;
	public int PlayerFragCount;
	public int PlayerHealth;
	public float Time;
	public int TowerCount;
	public int TowerFragCount;
	public int UserLevel;
}