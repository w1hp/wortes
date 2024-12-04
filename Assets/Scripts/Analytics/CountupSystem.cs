using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct CountupSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CountUpTime>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (countUpTime, entity) in SystemAPI.Query<RefRW<CountUpTime>>()
			.WithEntityAccess())
		{
			countUpTime.ValueRW.ElapsedTime += SystemAPI.Time.DeltaTime;
		}
	}
}

public struct CountUpTime : IComponentData
{
	public float ElapsedTime;
}