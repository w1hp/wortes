using Unity.Burst;
using Unity.Entities;

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

        foreach (var (drop, entity) in SystemAPI.Query<RefRO<Drop>>()
             .WithNone<IsExistTag>()
             .WithEntityAccess())
        {
            for (int i = 0; i < drop.ValueRO.ResourceAmount; i++)
            {
                var resource = state.EntityManager.Instantiate(drop.ValueRO.ResourcePrefab);
            }
            ECB.RemoveComponent<Drop>(entity);
        }

    }
}
