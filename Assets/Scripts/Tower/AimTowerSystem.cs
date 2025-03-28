using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct AimTowerSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		//state.RequireForUpdate<Tower>();
		state.RequireForUpdate<TowerAimer>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (towerAimer, target, transform) in 
			SystemAPI.Query<RefRW<TowerAimer>, RefRO<Target>, RefRW<LocalTransform>>())
		{
			if (target.ValueRO.Value == Entity.Null) break;

			var targetPosition = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Value).Position;

			// Aim at target
			if (math.distance(transform.ValueRO.Position, targetPosition) < towerAimer.ValueRO.Range)
			{
				towerAimer.ValueRW.IsEnemyInRange = true;
				float3 to = targetPosition;
				to.y = 0;
				float3 from = transform.ValueRO.Position;
				from.y = 0;
				var desiredRotation = TransformHelpers.LookAtRotation(from, to, math.up());

				var aimerTansform = SystemAPI.GetComponentRW<LocalTransform>(towerAimer.ValueRO.Aimer);
				aimerTansform.ValueRW.Rotation = math.slerp(aimerTansform.ValueRO.Rotation, desiredRotation, 0.08f);
			}
			else
			{
				towerAimer.ValueRW.IsEnemyInRange = false;
			}
		}
	}
}
