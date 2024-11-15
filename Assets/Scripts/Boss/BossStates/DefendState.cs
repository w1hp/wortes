using Unity.Entities;

public struct DefendState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Defend State");
#endif
	}

	public void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb)
	{

	}
}