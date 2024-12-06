using Unity.Burst;
using Unity.Entities;

partial struct CalculateTotalStatsSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{

	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (totalMoveSpeed, moveSpeedMod, entity) in
			SystemAPI.Query<RefRW<TotalMoveSpeed>, RefRO<MoveSpeedStatModification>>().WithEntityAccess())
		{
			totalMoveSpeed.ValueRW.Value += totalMoveSpeed.ValueRW.Value * moveSpeedMod.ValueRO.ModificationValue;
			ECB.RemoveComponent<MoveSpeedStatModification>(entity);
		}

		foreach (var (totalAttackSpeed, attackSpeedMod, entity) in
			SystemAPI.Query<RefRW<TotalAttackSpeed>, RefRO<AttackSpeedStatModification>>().WithEntityAccess())
		{
			totalAttackSpeed.ValueRW.Value += totalAttackSpeed.ValueRW.Value * attackSpeedMod.ValueRO.ModificationValue;
			ECB.RemoveComponent<AttackSpeedStatModification>(entity);
		}

		foreach (var (totalAttackDamage, attackDamageMod, entity) in
			SystemAPI.Query<RefRW<TotalAttackDamage>, RefRO<AttackDamageStatModification>>().WithEntityAccess())
		{
			totalAttackDamage.ValueRW.Value += attackDamageMod.ValueRO.ModificationValue;
			ECB.RemoveComponent<AttackDamageStatModification>(entity);
		}

		foreach (var (totalHealth, healthMod, entity) in
			SystemAPI.Query<RefRW<Health>, RefRO<HealthStatModification>>().WithEntityAccess())
		{
			totalHealth.ValueRW.MaxHealth += totalHealth.ValueRW.MaxHealth * healthMod.ValueRO.ModificationValue;
			ECB.RemoveComponent<HealthStatModification>(entity);
		}
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{

	}
}
