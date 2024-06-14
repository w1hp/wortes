//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Transforms;

//[BurstCompile]
//public partial struct ProjectileJob : IJobEntity
//{
//    public EntityCommandBuffer ECB;
//    public float DeltaTime;
//    [ReadOnly] public ComponentLookup<LocalTransform> LocalTransforms;
//    [ReadOnly] public ComponentLookup<EnemyComponent> EnemyComponents;
//    [ReadOnly] public EntityQuery EnemyQuery;

//    void Execute(Entity entity, ref Projectile projectile, ref LocalTransform transform)
//    {
//        var gravity = new float3(0.0f, -9.82f, 0.0f);

//        // Update position based on velocity and gravity
//        transform.Position += projectile.Velocity * DeltaTime;
//        projectile.Velocity += gravity * DeltaTime;

//        // Check if the projectile has hit the ground
//        if (transform.Position.y <= 0.0f)
//        {
//            ECB.DestroyEntity(entity);
//            return;
//        }

//        // Check for collision with enemies
//        var enemyEntities = EnemyQuery.ToEntityArray(Allocator.Temp);
//        foreach (var enemyEntity in enemyEntities)
//        {
//            if (math.distance(transform.Position, LocalTransforms[enemyEntity].Position) < 0.5f) // Assuming a collision radius
//            {
//                ECB.DestroyEntity(entity); // Destroy the projectile
//                ECB.DestroyEntity(enemyEntity); // Destroy the enemy
//                break;
//            }
//        }
//        enemyEntities.Dispose();
//    }
//}
