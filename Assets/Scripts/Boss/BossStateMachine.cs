using Unity.Entities;
using Unity.Mathematics;


public struct BossStateMachine : IComponentData
{
	public BossState CurrentState;
	public BossState PreviousState;
	public bool isSecondPhase;


	public float Timer;

	public IdleState IdleState;
	public MoveState MoveState;
	//public DashState DashState; 
	public DefendState DefendState;
	public AttackState AttackState;

	public void TransitionToState(BossState nextState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		PreviousState = CurrentState;
		CurrentState = nextState;

		OnStateExit(PreviousState, CurrentState, entity, ecb, ref systemState);
		OnStateEnter(CurrentState, PreviousState, entity, ecb, ref systemState);
	}

	public void OnStateExit(BossState state, BossState newState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateExit(newState, entity, ecb, ref systemState);
				break;
			case BossState.Move:
				MoveState.OnStateExit(newState, entity, ecb, ref systemState);
				break;
			//case BossState.Dash:
			//	DashState.OnStateExit(newState, entity, ref systemState);
			//	break;
			case BossState.Defend:
				DefendState.OnStateExit(newState, entity, ecb, ref systemState);
				break;
			case BossState.Attack:
				AttackState.OnStateExit(newState, entity, ecb, ref systemState);
				break;
		}
	}

	public void OnStateEnter(BossState state, BossState previousState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateEnter(previousState, entity, ecb, ref systemState);
				break;
			case BossState.Move:
				MoveState.OnStateEnter(previousState, entity, ecb, ref systemState);
				break;
			//case BossState.Dash:
			//	DashState.OnStateEnter(previousState, entity, ref systemState);
			//	break;
			case BossState.Defend:
				DefendState.OnStateEnter(previousState, entity, ecb, ref systemState);
				break;
			case BossState.Attack:
				AttackState.OnStateEnter(previousState, entity, ecb, ref systemState);
				break;
		}
	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		switch (CurrentState)
		{
			case BossState.Idle:
				IdleState.OnStateUpdate(bossStateMachine, entity, ecb, ref systemState);
				break;
			case BossState.Move:
				MoveState.OnStateUpdate(bossStateMachine, entity, ecb, ref systemState);
				break;
			//case BossState.Dash:
			//	DashState.OnStateUpdate(bossStateMachine, entity, ref state);
			//	break;
			case BossState.Defend:
				DefendState.OnStateUpdate(bossStateMachine, entity, ecb, ref systemState);
				break;
			case BossState.Attack:
				AttackState.OnStateUpdate(bossStateMachine, entity, ecb, ref systemState);
				break;
		}
	}

}


