using Unity.Entities;

public struct IdleState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Idle State");
#endif

	}

	public void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb)
	{
		if (bossStateMachine.ValueRO.PreviousState == BossState.Attack)
			bossStateMachine.ValueRW.TransitionToState(BossState.Move, entity, ecb);
		else
			bossStateMachine.ValueRW.TransitionToState(BossState.Attack, entity, ecb);
	}
}