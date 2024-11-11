using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct CountupSystem : ISystem
{
	int lvl;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CountUpTime>();

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

		foreach (var (countUpTime, entity) in SystemAPI.Query<RefRW<CountUpTime>>()
			.WithEntityAccess())
		{
			countUpTime.ValueRW.ElapsedTime += SystemAPI.Time.DeltaTime;
			screenSpaceUISingleton.UpdateTimer(ConvertToMinutesAndSeconds(ref state, countUpTime.ValueRO.ElapsedTime));
			if (countUpTime.ValueRO.ElapsedTime >= 60 * lvl)
			{
				lvl++;
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

public struct CountUpTime : IComponentData
{
	public float ElapsedTime;
}

//[BurstCompile]
//public partial struct ProcessSpawnerJob : IJobEntity
//{
//	public EntityCommandBuffer.ParallelWriter Ecb;
//	public double ElapsedTime = SystemAPI.Time.ElapsedTime;

//	private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spawner spawner)
//	{
//		// If the next spawn time has passed.
//		if (spawner.NextSpawnTime < ElapsedTime)
//		{
//			// Spawns a new entity and positions it at the spawner.
//			Entity newEntity = Ecb.Instantiate(chunkIndex, spawner.Prefab);
//			Ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawner.SpawnPosition));

//			// Resets the next spawn time.
//			spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
//		}
//	}
//}