using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Stateful;
using Unity.Assertions;


partial struct EnemyCollisionImpactSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EnemyComponent>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var projectileQuery = SystemAPI.QueryBuilder()
			.WithAll<Projectile>()
			.Build();

		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		// Assert.IsFalse(nonTriggerQuery.HasFilter(),
		//     "The use of EntityQueryMask in this system will not respect the query's active filter settings.");

		var projectileMask = projectileQuery.GetEntityQueryMask();

		foreach (var (enemy, health, collisionEventBuffer, entity) in
			SystemAPI.Query<RefRW<EnemyComponent>, RefRW<Health>, DynamicBuffer<StatefulCollisionEvent>>()
			.WithEntityAccess())
		{
			for (int i = 0; i < collisionEventBuffer.Length; i++)
			{
				var collisionEvent = collisionEventBuffer[i];
				var otherEntity = collisionEvent.GetOtherEntity(entity);

				if (!projectileMask.MatchesIgnoreFilter(otherEntity)) continue;

				switch (collisionEvent.State)
				{
					case StatefulEventState.Enter:
						var projectile = SystemAPI.GetComponent<Projectile>(otherEntity);
						health.ValueRW.TakeDamage(projectile.Damage, projectile.Type);

						if (health.ValueRW.CurrentHealth <= 0f)
						{
							ECB.SetComponentEnabled<IsExistTag>(entity, false);
						}
						break;
					case StatefulEventState.Stay:
						break;
					case StatefulEventState.Exit:
						break;
				}
			}
		}
	}
}
