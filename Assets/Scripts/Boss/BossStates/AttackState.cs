using Unity.Entities;

public struct AttackState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, ref SystemState state)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Attack State");
#endif
	}

	public void OnStateExit(BossState nextState, Entity entity, ref SystemState state)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, ref SystemState state)
	{
		bossStateMachine.ValueRW.TransitionToState(BossState.Idle, entity, ref state);
	}
}