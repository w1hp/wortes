using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(EnemyCollisionImpactSystem))]
partial struct DropSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Drop>();
	}

	[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (drop, transform, entity) in SystemAPI.Query<RefRO<Drop>, LocalTransform>()
             .WithNone<IsExistTag>()
             .WithEntityAccess())
        {
            for (int i = 0; i < drop.ValueRO.ResourceAmount; i++)
            {
                var resource = state.EntityManager.Instantiate(drop.ValueRO.ResourcePrefab);
				ECB.SetComponent(resource, transform);

                PhysicsVelocity velocity = new PhysicsVelocity
                {
                    Linear = 1f,
                    Angular = new float3 { xyz = i * 30 }
				};
				ECB.SetComponent(resource, velocity);
			}
			// TODO: tutaj mozna dodac licznik zabitych przeciwnikow
			ECB.DestroyEntity(entity);
        }

    }
}
