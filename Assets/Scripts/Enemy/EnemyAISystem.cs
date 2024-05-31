using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public partial struct EnemyAISystem : ISystem
{
    private EntityManager entityManager;
    private Entity playerEntity;

    private void OnUpdate(ref SystemState state)
    {
        entityManager = state.EntityManager;
        playerEntity = SystemAPI.GetSingletonEntity<Player>();

        foreach (var (enemyComponent, transformComponent) in SystemAPI.Query<EnemyComponent, RefRW<LocalTransform>>())
        {
            float3 direction = entityManager.GetComponentData<LocalTransform>(playerEntity).Position - transformComponent.ValueRO.Position;
            float angle = math.atan2(direction.z, direction.x);
            transformComponent.ValueRW.Rotation = quaternion.Euler(new float3(0, angle, 0));

            transformComponent.ValueRW.Position += math.normalize(direction) * SystemAPI.Time.DeltaTime;
        }
    }
}