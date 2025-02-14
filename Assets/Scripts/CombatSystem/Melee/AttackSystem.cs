using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
partial struct MeleeAttackSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<AttackComponent>();
	}
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (meleeAttackComponent, transform, entity) in SystemAPI.Query<RefRW<AttackComponent>, LocalToWorld>().WithEntityAccess())
		{
			var graphicsRepresentation = meleeAttackComponent.ValueRO.GraphicsRepresentation;

			meleeAttackComponent.ValueRW.LastAttackTime -= SystemAPI.Time.DeltaTime;

			if (meleeAttackComponent.ValueRO.LastAttackTime > 0)
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(graphicsRepresentation, false);
				return;

			}


			var filter = new CollisionFilter()
			{
				BelongsTo = ~0u,
				CollidesWith = 1 << 5,
				GroupIndex = 0
			};

			SystemAPI.SetComponentEnabled<MaterialMeshInfo>(graphicsRepresentation, true);

			NativeList<DistanceHit> distances = new NativeList<DistanceHit>(Allocator.Temp);

			if (physicsWorldSingleton.OverlapBox(transform.Position, transform.Rotation, meleeAttackComponent.ValueRO.AttackRange, ref distances, filter))
			{
				foreach (var hit in distances)
				{
#if UNITY_EDITOR
					UnityEngine.Debug.Log($"Hit: {hit.Entity}");
#endif
					ecb.AddComponent(hit.Entity, new DamageToCharacter
					{
						Type = meleeAttackComponent.ValueRO.ElementType,
						Value = meleeAttackComponent.ValueRO.AttackDamage,
						OriginCharacterType = meleeAttackComponent.ValueRO.OriginCharacterType
					});
				}
			}

			meleeAttackComponent.ValueRW.LastAttackTime = meleeAttackComponent.ValueRO.AttackCooldown;

		}
	}
}
