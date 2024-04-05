using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct CharacterMovementSystem : ISystem
{
	Entity inputEntity;
	InputsData inputsData;
	public void OnUpdate(ref SystemState state)
	{
		inputEntity = SystemAPI.GetSingletonEntity<InputsData>();
		inputsData = state.EntityManager.GetComponentData<InputsData>(inputEntity);

		foreach (var (data, transform) in SystemAPI.Query<RefRO<Character>, RefRW<LocalTransform>>())
		{
			float3 position = transform.ValueRO.Position;
			position.x += inputsData.move.x * data.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
			position.z += inputsData.move.y * data.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
			transform.ValueRW.Position = position;
		}
	}
}
