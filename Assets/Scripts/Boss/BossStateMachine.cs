using Unity.Entities;
using Unity.Mathematics;


public struct BossStateMachine : IComponentData
{
	public BossState CurrentState;
	public BossState PreviousState;

	public float Timer;
	public uint Seed;
	//TASK: Set Seed and Timer field in baker
	public IdleState IdleState;
	public MoveState MoveState;
	//public DashState DashState; // wait with this implementation
	public DefendState DefendState;
	public AttackState AttackState;

	public void TransitionToState(BossState nextState, Entity entity, ref SystemState systemState)
	{
		PreviousState = CurrentState;
		CurrentState = nextState;

		Random rng = new Random(Seed);
		Seed = rng.NextUInt(10);
		Timer = rng.NextFloat(0, 4);

		OnStateExit(PreviousState, CurrentState, entity, ref systemState);
		OnStateEnter(CurrentState, PreviousState, entity, ref systemState);
	}

	public void OnStateExit(BossState state, BossState newState, Entity entity, ref SystemState systemState)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateExit(newState, entity, ref systemState);
				break;
			case BossState.Move:
				MoveState.OnStateExit(newState, entity, ref systemState);
				break;
			//case BossState.Dash:
			//	DashState.OnStateExit(newState, entity, ref systemState);
			//	break;
			case BossState.Defend:
				DefendState.OnStateExit(newState, entity, ref systemState);
				break;
			case BossState.Attack:
				AttackState.OnStateExit(newState, entity, ref systemState);
				break;
		}
	}

	public void OnStateEnter(BossState state, BossState previousState, Entity entity, ref SystemState systemState)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateEnter(previousState, entity, ref systemState);
				break;
			case BossState.Move:
				MoveState.OnStateEnter(previousState, entity, ref systemState);
				break;
			//case BossState.Dash:
			//	DashState.OnStateEnter(previousState, entity, ref systemState);
			//	break;
			case BossState.Defend:
				DefendState.OnStateEnter(previousState, entity, ref systemState);
				break;
			case BossState.Attack:
				AttackState.OnStateEnter(previousState, entity, ref systemState);
				break;
		}
	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, ref SystemState state)
	{
		switch (CurrentState)
		{
			case BossState.Idle:
				IdleState.OnStateUpdate(bossStateMachine, entity, ref state);
				break;
			case BossState.Move:
				MoveState.OnStateUpdate(bossStateMachine, entity, ref state);
				break;
			//case BossState.Dash:
			//	DashState.OnStateUpdate(bossStateMachine, entity, ref state);
			//	break;
			case BossState.Defend:
				DefendState.OnStateUpdate(bossStateMachine, entity, ref state);
				break;
			case BossState.Attack:
				AttackState.OnStateUpdate(bossStateMachine, entity, ref state);
				break;
		}
	}

}


