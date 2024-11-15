using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum BossState
{
	Uninitialized,

	Idle,
	Move,
	Dash,
	Defend,
	Attack,
}
public interface IBossState
{
	void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb);
	void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb);
	void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb);
}





