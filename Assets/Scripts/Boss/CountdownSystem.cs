using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct CountdownSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<BossCountdown>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		if (ScreenSpaceUIController.Singleton == null)
		{
			return;
		}
		var screenSpaceUISingleton = ScreenSpaceUIController.Singleton;

		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (bossCountdown, entity) in SystemAPI.Query<RefRW<BossCountdown>>()
			.WithEntityAccess())
		{
			bossCountdown.ValueRW.TargetDuration = bossCountdown.ValueRO.TargetDuration > 0 ?
				bossCountdown.ValueRW.TargetDuration -= SystemAPI.Time.DeltaTime : 0;
			screenSpaceUISingleton.UpdateTimer(ConvertToMinutesAndSeconds(ref state, bossCountdown.ValueRO.TargetDuration));
			if (bossCountdown.ValueRO.TargetDuration == 0)
			{
				ECB.RemoveComponent<BossCountdown>(entity);
			}
		}
	}
	[BurstCompile]
	(float minutes, float seconds) ConvertToMinutesAndSeconds(ref SystemState state, float time)
	{
		float minutes = math.floor(time / 60);
		float seconds = math.floor(time % 60);
		return (minutes, seconds);
	}
}

//TODO: add boss manager
