using Unity.Burst;
using Unity.Entities;

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
		foreach (var (bossStateMachine, entity) in SystemAPI.Query<BossStateMachine>()
            .WithEntityAccess())
		{
            bossStateMachine.OnStateUpdate(entity, ref state);
		}
	}

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
