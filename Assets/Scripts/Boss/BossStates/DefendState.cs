using Unity.Entities;

public struct DefendState : IBossState
{
	private float currentHealth;
	private float maxHealth;
	public void OnStateEnter(BossState previousState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.Log("Entering Defend State");
#endif
		var health = systemState.EntityManager.GetComponentData<Health>(entity);
		currentHealth = health.CurrentHealth;
		maxHealth = health.MaxHealth;
		ecb.SetComponent(entity, new Health
		{
			CurrentHealth = health.CurrentHealth,
			MaxHealth = health.MaxHealth,
			FireResistance = 100,
			WaterResistance = 100,
			EarthResistance = 100,
			WoodResistance = 100,
			MetalResistance = 100,
		});
		systemState.WorldUnmanaged.GetExistingSystemState<EnemySpawnerSystem>().Enabled = true;
	}

	public void OnStateExit(BossState nextState, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		ecb.SetComponent(entity, new Health
		{
			CurrentHealth = currentHealth,
			MaxHealth = maxHealth,
			FireResistance = 0,
			WaterResistance = 0,
			EarthResistance = 0,
			WoodResistance = 0,
			MetalResistance = 0,
		});
		systemState.WorldUnmanaged.GetExistingSystemState<EnemySpawnerSystem>().Enabled = false;
	}

	public void OnStateUpdate(RefRW<BossStateMachine> bossStateMachine, Entity entity, EntityCommandBuffer ecb, ref SystemState systemState)
	{
		bossStateMachine.ValueRW.TransitionToState(BossState.Idle, entity, ecb, ref systemState);
	}
}