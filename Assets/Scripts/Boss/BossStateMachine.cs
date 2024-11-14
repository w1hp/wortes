using Unity.Entities;

public struct BossStateMachine : IComponentData
{
	public BossState CurrentState;
	public BossState PreviousState;
	public BossState BossPhase;

	public IdleState IdleState;
	public MoveState MoveState;
	public DashState DashState;
	public DefendState DefendState;
	public AttackState AttackState;
	public DeathState DeathState;

	public void TransitionToState(BossState nextState, BossPhase bossPhase)
	{
		PreviousState = CurrentState;
		CurrentState = nextState;

		OnStateExit(PreviousState, CurrentState, bossPhase);
		OnStateEnter(CurrentState, PreviousState, bossPhase);
	}

	public void OnStateExit(BossState state, BossState newState, BossPhase bossPhase)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateExit(newState, bossPhase);
				break;
			case BossState.Move:
				MoveState.OnStateExit(newState, bossPhase);
				break;
			case BossState.Dash:
				DashState.OnStateExit(newState, bossPhase);
				break;
			case BossState.Defend:
				DefendState.OnStateExit(newState, bossPhase);
				break;
			case BossState.Attack:
				AttackState.OnStateExit(newState, bossPhase);
				break;
			case BossState.Death:
				DeathState.OnStateExit(newState, bossPhase);
				break;
		}
	}

	public void OnStateEnter(BossState state, BossState previousState, BossPhase bossPhase)
	{
		switch (state)
		{
			case BossState.Idle:
				IdleState.OnStateEnter(previousState, bossPhase);
				break;
			case BossState.Move:
				MoveState.OnStateEnter(previousState, bossPhase);
				break;
			case BossState.Dash:
				DashState.OnStateEnter(previousState, bossPhase);
				break;
			case BossState.Defend:
				DefendState.OnStateEnter(previousState, bossPhase);
				break;
			case BossState.Attack:
				AttackState.OnStateEnter(previousState, bossPhase);
				break;
			case BossState.Death:
				DeathState.OnStateEnter(previousState, bossPhase);
				break;
		}
	}

	public void OnStateUpdate(Entity entity, ref SystemState state)
	{
		switch (CurrentState)
		{
			case BossState.Idle:
				IdleState.OnStateUpdate(entity, ref state);
				break;
			case BossState.Move:
				MoveState.OnStateUpdate(entity, ref state);
				break;
			case BossState.Dash:
				DashState.OnStateUpdate(entity, ref state);
				break;
			case BossState.Defend:
				DefendState.OnStateUpdate(entity, ref state);
				break;
			case BossState.Attack:
				AttackState.OnStateUpdate(entity, ref state);
				break;
			case BossState.Death:
				DeathState.OnStateUpdate(entity, ref state);
				break;
		}
	}

}


