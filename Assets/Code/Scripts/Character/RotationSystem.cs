using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct RotationSystem : ISystem
{
	Entity inputEntity;
	InputsData inputsData;
	public void OnUpdate(ref SystemState state)
	{
		inputEntity = SystemAPI.GetSingletonEntity<InputsData>();
		inputsData = state.EntityManager.GetComponentData<InputsData>(inputEntity);

		foreach (var (data, localTransform, worldTransform) in SystemAPI.Query<RefRO<CharacterModel>, RefRW<LocalTransform>, RefRW<LocalToWorld>>())
		{
			Vector2 dir = (Vector2)inputsData.look; // - (Vector2)Camera.main.WorldToScreenPoint(worldTransform.ValueRO.Position);
			float angle = math.degrees(math.atan2(dir.y * -1, dir.x));
			localTransform.ValueRW.Rotation = quaternion.RotateY(math.radians(angle));
			//localTransform.ValueRW.Rotation = Quaternion.AngleAxis(angle, Vector3.up);
			////localTransform.ValueRW.Rotation = quaternion.AxisAngle(math.up(), angle * SystemAPI.Time.DeltaTime);

			//float angle = math.atan2(inputY, inputX);
			//quaternion rotation = quaternion.RotateY(angle);
			//transform.ValueRW.Rotation = math.mul(transform.ValueRW.Rotation, rotation);


			//var target = new float3(inputsData.look.x, 0, inputsData.look.y);
			//quaternion rotation = quaternion.LookRotation(math.normalize(target), math.up());
			//localTransform.ValueRW.Rotation = rotation;
		}
	}
}