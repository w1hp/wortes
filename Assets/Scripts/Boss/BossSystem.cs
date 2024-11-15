using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;



partial struct BossSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate(SystemAPI.QueryBuilder()
			.WithAll<BossStateMachine>()
			.WithNone<BossInitialization>()
			.Build());
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (bossStateMachine, health, entity) in
			SystemAPI.Query<RefRW<BossStateMachine>, Health>()
			.WithEntityAccess())
		{
			//if (health.CurrentHealth <= (health.MaxHealth / 2))
			//	bossStateMachine.ValueRW.TransitionToState(BossState.Defend);

	

			// Update timer
			bossStateMachine.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
			if (bossStateMachine.ValueRW.Timer <= 0)
			{
				bossStateMachine.ValueRW.OnStateUpdate(bossStateMachine, entity, ref state);
			}
		}
	}
}
