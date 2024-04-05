using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct CameraSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (data, transform) in SystemAPI.Query<RefRO<Character>, RefRW<LocalTransform>>())
		{
			var cameraTransform = Camera.main.transform;
			cameraTransform.position = transform.ValueRO.Position;
			cameraTransform.position -= 10.0f * (Vector3)transform.ValueRO.Forward(); 
			cameraTransform.position += new Vector3(0, 10f, 0);  
		}
	}
}