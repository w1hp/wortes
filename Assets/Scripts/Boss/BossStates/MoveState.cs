using Unity.Entities;

public struct MoveState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, ref SystemState state)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Move State");
#endif
		
		state.EntityManager.AddComponent<EnemyComponent>(entity);

	}

	public void OnStateExit(BossState nextState, Entity entity, ref SystemState state)
	{
		state.EntityManager.RemoveComponent<EnemyComponent>(entity);
	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, ref SystemState state)
	{
		bossStateMachine.ValueRW.TransitionToState(BossState.Idle, entity, ref state);
	}
}
