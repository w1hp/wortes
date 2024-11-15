using Unity.Entities;
using Unity.Mathematics;


public struct BossStateMachine : IComponentData
{
	public BossState CurrentState;
	public BossState PreviousState;

	public float Timer;
	//public uint Seed;
	//TASK: Set Seed and Timer field in baker
	public IdleState IdleState;
	public MoveState MoveState;
	//public DashState DashState; // wait with this implementation
	public DefendState DefendState;
	public AttackState AttackState;

	public void TransitionToState(BossState nextState, Entity entity, EntityCommandBuffer ecb)
	{
		PreviousState = CurrentState;
		CurrentState = nextState;

		//Random rng = new Random(Seed);
		//Seed = rng.NextUInt(1,666);

		//Random rng = new Random();
		//Timer = rng.NextFloat(1, 7);

		OnStateExit(PreviousState, CurrentState, entity, ecb);
		OnStateEnter(CurrentState, PreviousState, entity, ecb);
	}

	public void OnStateExit(BossState state, BossState newState, Entity entity, EntityCommandBuffer ecb)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateExit(newState, entity, ecb);
				break;
			case BossState.Move:
				MoveState.OnStateExit(newState, entity, ecb);
				break;
			//case BossState.Dash:
			//	DashState.OnStateExit(newState, entity, ref systemState);
			//	break;
			case BossState.Defend:
				DefendState.OnStateExit(newState, entity, ecb);
				break;
			case BossState.Attack:
				AttackState.OnStateExit(newState, entity, ecb);
				break;
		}
	}

	public void OnStateEnter(BossState state, BossState previousState, Entity entity, EntityCommandBuffer ecb)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateEnter(previousState, entity, ecb);
				break;
			case BossState.Move:
				MoveState.OnStateEnter(previousState, entity, ecb);
				break;
			//case BossState.Dash:
			//	DashState.OnStateEnter(previousState, entity, ref systemState);
			//	break;
			case BossState.Defend:
				DefendState.OnStateEnter(previousState, entity, ecb);
				break;
			case BossState.Attack:
				AttackState.OnStateEnter(previousState, entity, ecb);
				break;
		}
	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb)
	{
		switch (CurrentState)
		{
			case BossState.Idle:
				IdleState.OnStateUpdate(bossStateMachine, entity, ecb);
				break;
			case BossState.Move:
				MoveState.OnStateUpdate(bossStateMachine, entity, ecb);
				break;
			//case BossState.Dash:
			//	DashState.OnStateUpdate(bossStateMachine, entity, ref state);
			//	break;
			case BossState.Defend:
				DefendState.OnStateUpdate(bossStateMachine, entity, ecb);
				break;
			case BossState.Attack:
				AttackState.OnStateUpdate(bossStateMachine, entity, ecb);
				break;
		}
	}

}


