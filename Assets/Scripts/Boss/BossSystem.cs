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

		rng = new Random(1234);
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
			if (!bossStateMachine.ValueRO.isSecondPhase && health.CurrentHealth <= (health.MaxHealth / 2))
			{
				bossStateMachine.ValueRW.TransitionToState(BossState.Defend, entity, ECB, ref state);
				bossStateMachine.ValueRW.isSecondPhase = true;
				bossStateMachine.ValueRW.Timer = 15f;
			}

			// Update timer
			bossStateMachine.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
			if (bossStateMachine.ValueRW.Timer <= 0)
			{
				if (bossStateMachine.ValueRW.CurrentState != BossState.Idle)
					bossStateMachine.ValueRW.Timer = rng.NextFloat(.5f, 1.5f);
				else
					bossStateMachine.ValueRW.Timer = rng.NextFloat(4, 7);

				bossStateMachine.ValueRW.OnStateUpdate(bossStateMachine, entity, ECB, ref state);
			}
		}
	}
}
