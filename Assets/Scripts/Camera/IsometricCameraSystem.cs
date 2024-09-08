using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

partial struct IsometricCameraSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var characterQuery = SystemAPI.QueryBuilder().WithAll<CharacterComponent>().WithNone<Prefab>().Build();
        var characterArr = characterQuery.ToEntityArray(Allocator.Temp);
		foreach (var (data, transform) in SystemAPI.Query<RefRO<IsometricCamera>, RefRW<LocalTransform>>())
		{
			var characterTransform = SystemAPI.GetComponentRO<LocalTransform>(characterArr[0]);
            var newPosiotion = characterTransform.ValueRO.Position;
			newPosiotion.y = 10f;
			newPosiotion.z -= 10f;

			transform.ValueRW.Position = newPosiotion;
            transform.ValueRW.Rotation = quaternion.Euler(.8f, 0, 0);
		}
	}

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
