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

	public void OnUpdate(ref SystemState state)
	{
		if (CountdownController.Singleton == null)
		{
			return;
		}
		var countdownController = CountdownController.Singleton;

		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (bossCountdown, entity) in SystemAPI.Query<RefRW<BossCountdown>>()
			.WithEntityAccess())
		{
			bossCountdown.ValueRW.TargetDuration = bossCountdown.ValueRO.TargetDuration > 0 ?
				bossCountdown.ValueRW.TargetDuration -= SystemAPI.Time.DeltaTime : 0;
			countdownController.UpdateTimer(ConvertToMinutesAndSeconds(ref state, bossCountdown.ValueRO.TargetDuration));
			if (bossCountdown.ValueRO.TargetDuration == 0)
			{
				countdownController.EnableBossHealthPanel();
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