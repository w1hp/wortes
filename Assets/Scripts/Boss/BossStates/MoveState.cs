using Unity.Entities;

public struct MoveState : IBossState
{
	public void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Move State");
#endif
		ecb.AddComponent(entity, new EnemyComponent
		{
			EnemyType = ElementType.Fire,
			DetectionRange = 30f,
			Damage = 25f
		});

	}

	public void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		ecb.RemoveComponent<EnemyComponent>(entity);
	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		bossStateMachine.ValueRW.TransitionToState(BossState.Idle, entity, ecb, ref systemState);
	}
}
