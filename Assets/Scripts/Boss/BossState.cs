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
	void OnStateEnter(BossState previousState, Entity entity, ref SystemState state);
	void OnStateExit(BossState nextState, Entity entity, ref SystemState state);
	void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, ref SystemState state);
}





