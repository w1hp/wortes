using Unity.Entities;

public struct IdleState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, ref SystemState state)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Idle State");
#endif

	}

	public void OnStateExit(BossState nextState, Entity entity, ref SystemState state)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, ref SystemState state)
	{
		if (bossStateMachine.ValueRO.PreviousState == BossState.Attack)
			bossStateMachine.ValueRW.TransitionToState(BossState.Move, entity, ref state);
		else
			bossStateMachine.ValueRW.TransitionToState(BossState.Attack, entity, ref state);
	}
}