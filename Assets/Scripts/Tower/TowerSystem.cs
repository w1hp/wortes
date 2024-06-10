using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct TowerSystem : ISystem
{

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Tower>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (tower, target, transform) in SystemAPI.Query<Tower, RefRO<Target>, RefRW<LocalTransform>>())
		{
			if (target.ValueRO.Value == Entity.Null) break;

			var targetPosition = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Value).Position;

			// Aim at target
			if (math.distance(transform.ValueRO.Position, targetPosition) < tower.Range)
			{
				// Normalize y axis OR
				float3 to = targetPosition;
				to.y = 0;
				float3 from = transform.ValueRO.Position;
				from.y = 0;
				var desiredRotation = TransformHelpers.LookAtRotation(from, to, math.up());

				// not normalized y axis
				//var desiredRotation = TransformHelpers.LookAtRotation(transform.ValueRO.Position, targetPosition, math.up());

				var aimerTansform = SystemAPI.GetComponentRW<LocalTransform>(tower.Aimer);
				aimerTansform.ValueRW.Rotation = math.slerp(aimerTansform.ValueRO.Rotation, desiredRotation, 0.08f);
			}

			// Shoot

		}
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{

	}
}
