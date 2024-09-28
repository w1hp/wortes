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
		
		
		var enemyQuery = SystemAPI.QueryBuilder()
			.WithAll<EnemyTag>()
			.Build();


		var projectileJob = new ProjectileJob
		{
			ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
			enemyMask = enemyQuery.GetEntityQueryMask()
		};

		projectileJob.Schedule();
	}
}

[BurstCompile]
public partial struct ProjectileJob : IJobEntity
{
	public EntityCommandBuffer ECB;
	public EntityQueryMask enemyMask;

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
					if (enemyMask.MatchesIgnoreFilter(otherEntity))
					{
						ECB.AddComponent(otherEntity, new DamageToCharacter
						{
							Value = projectile.Damage,
							Type = projectile.Type,
							OriginCharacter = projectile.OriginCharacter
						});
					}
					ECB.DestroyEntity(entity);
					break;
				case StatefulEventState.Exit:
					break;
			}
		}
	}
}
