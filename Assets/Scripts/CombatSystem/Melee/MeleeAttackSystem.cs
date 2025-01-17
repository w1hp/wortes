using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
partial struct MeleeAttackSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{	
		var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
		
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (meleeAttackComponent, transform, entity) in SystemAPI.Query<MeleeAttackComponent, LocalToWorld>().WithEntityAccess())
		{
			var filter = new CollisionFilter()
			{
				BelongsTo = ~0u,
				CollidesWith = 1 << 5,
				GroupIndex = 0
			};

			NativeList<DistanceHit> distances = new NativeList<DistanceHit>(Allocator.Temp);

			if (physicsWorldSingleton.OverlapSphere(transform.Position + meleeAttackComponent.PositionOverlap, meleeAttackComponent.Radius, ref distances, filter))
			{
				foreach (var hit in distances)
				{
					UnityEngine.Debug.Log("Hit: " + hit.Entity);
					ecb.AddComponent(hit.Entity, new DamageToCharacter
					{
						Type = meleeAttackComponent.ElementType,
						Value = meleeAttackComponent.AttackDamage,
						OriginCharacterType = meleeAttackComponent.OriginCharacterType
					});
				}
			}
		}
	}
}
