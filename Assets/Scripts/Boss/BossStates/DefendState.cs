using Unity.Entities;

public struct DefendState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, ref SystemState state)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Defend State");
#endif
	}

	public void OnStateExit(BossState nextState, Entity entity, ref SystemState state)
	{

	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, ref SystemState state)
	{

	}
}