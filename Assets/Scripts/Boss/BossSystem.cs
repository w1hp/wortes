using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;



partial struct BossSystem : ISystem
{
	private Random rng;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate(SystemAPI.QueryBuilder()
			.WithAll<BossStateMachine>()
			.WithNone<BossInitialization>()
			.Build());

		rng = new Random(666);
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		EntityCommandBuffer ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (bossStateMachine, health, entity) in
			SystemAPI.Query<RefRW<BossStateMachine>, Health>()
			.WithEntityAccess())
		{
			if (health.CurrentHealth <= 0)
			{
				ECB.SetComponentEnabled<IsExistTag>(entity, false);
			}
			if (health.CurrentHealth <= (health.MaxHealth / 2))
				bossStateMachine.ValueRW.TransitionToState(BossState.Defend, entity, ECB);



			// Update timer
			bossStateMachine.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
			if (bossStateMachine.ValueRW.Timer <= 0)
			{
				bossStateMachine.ValueRW.Timer = rng.NextFloat(.5f, 3);
				bossStateMachine.ValueRW.OnStateUpdate(bossStateMachine, entity, ECB);
			}
		}
	}
}
