using Unity.Entities;

public struct AttackState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Attack State");
#endif
	}

	public void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		bossStateMachine.ValueRW.TransitionToState(BossState.Idle, entity, ecb, ref systemState);
	}
}