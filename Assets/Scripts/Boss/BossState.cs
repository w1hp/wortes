using Unity.Entities;
using Unity.Mathematics;

public enum BossPhase
{
	Phase_1,
	Phase_2,
	Phase_3,
}

public enum BossState
{
	Uninitialized,

	Idle,
	Move,
	Dash,
	Defend,
	Attack,


	Death,
}
public interface IBossState
{
	void OnStateEnter(BossState previousState, BossPhase bossPhase);
	void OnStateUpdate(Entity entity, ref SystemState state);
	void OnStateExit(BossState nextState, BossPhase bossPhase);
}

public struct IdleState : IBossState
{
	public void OnStateEnter(BossState previousState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateExit(BossState nextState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateUpdate(Entity entity, ref SystemState state)
	{
		throw new System.NotImplementedException();
	}
}
public struct MoveState : IBossState
{
	public void OnStateEnter(BossState previousState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateExit(BossState nextState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateUpdate(Entity entity, ref SystemState state)
	{
		state.EntityManager.AddComponent<EnemyComponent>(entity);
		//throw new System.NotImplementedException();
	}
}
public struct DashState : IBossState
{
	public void OnStateEnter(BossState previousState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateExit(BossState nextState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateUpdate(Entity entity, ref SystemState state)
	{
		throw new System.NotImplementedException();
	}
}
public struct DefendState : IBossState
{
	public void OnStateEnter(BossState previousState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateExit(BossState nextState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateUpdate(Entity entity, ref SystemState state)
	{
		throw new System.NotImplementedException();
	}
}
public struct AttackState : IBossState
{
	public void OnStateEnter(BossState previousState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateExit(BossState nextState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateUpdate(Entity entity, ref SystemState state)
	{
		throw new System.NotImplementedException();
	}
}
public struct DeathState : IBossState
{
	public void OnStateEnter(BossState previousState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateExit(BossState nextState, BossPhase bossPhase)
	{
		throw new System.NotImplementedException();
	}

	public void OnStateUpdate(Entity entity, ref SystemState state)
	{
		throw new System.NotImplementedException();
	}
}