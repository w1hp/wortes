using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Stateful;
using Unity.Transforms;

public partial struct ProjectileSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Projectile>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

		var projectileJob = new ProjectileJob
		{
			ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
			//DeltaTime = SystemAPI.Time.DeltaTime
		};

		projectileJob.Schedule();
	}
}

[BurstCompile]
public partial struct ProjectileJob : IJobEntity
{
	public EntityCommandBuffer ECB;
	//public float DeltaTime;

	void Execute(
		Entity entity, 
		ref Projectile projectile,
		in DynamicBuffer<StatefulCollisionEvent> collisionEventBuffer,
		ref LocalTransform transform)
	{
		for (int i = 0; i < collisionEventBuffer.Length; i++)
		{
			var collisionEvent = collisionEventBuffer[i];
			var otherEntity = collisionEvent.GetOtherEntity(entity);

			switch (collisionEvent.State)
			{
				case StatefulEventState.Enter:
					break;
				case StatefulEventState.Stay:
					ECB.DestroyEntity(entity);
					break;
				case StatefulEventState.Exit:
					break;
			}
		}
		//var gravity = new float3(0.0f, -9.82f, 0.0f);

		//transform.Position += projectile.Velocity * DeltaTime;

		//if (transform.Position.y <= 0.0f)
		//{
		//	ECB.DestroyEntity(entity);
		//}

		//projectile.Velocity += gravity * DeltaTime;



	}
}
