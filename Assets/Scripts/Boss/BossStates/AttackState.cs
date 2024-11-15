using Unity.Entities;

public struct AttackState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Attack State");
#endif
	}

	public void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb)
	{
		bossStateMachine.ValueRW.TransitionToState(BossState.Idle, entity, ecb);
	}
}