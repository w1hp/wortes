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