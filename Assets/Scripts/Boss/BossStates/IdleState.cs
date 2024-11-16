using Unity.Entities;

public struct IdleState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Idle State");
#endif

	}

	public void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		if (bossStateMachine.ValueRO.PreviousState == BossState.Attack)
			bossStateMachine.ValueRW.TransitionToState(BossState.Move, entity, ecb, ref systemState);
		else
			bossStateMachine.ValueRW.TransitionToState(BossState.Attack, entity, ecb, ref systemState);
	}
}