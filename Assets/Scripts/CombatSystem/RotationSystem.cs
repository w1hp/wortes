using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;
using Unity.Collections;

partial struct RotationSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();

		EntityQuery singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
		var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
		singletonQuery.Dispose();

		//var physicsWorld = SystemAPI.GetSingleton<BuildPhysicsWorld>().PhysicsWorld;
		//var collisionWorld = physicsWorld.CollisionWorld;

		foreach (var (characterComponent, characterControl, transform) in SystemAPI.Query<CharacterComponent, CharacterControl, LocalToWorld>())
		{
			var gunTransform = SystemAPI.GetComponent<LocalTransform>(characterComponent.GunPrefabEntity);

			// Raycast from mouse position
			var ray = Camera.main.ScreenPointToRay(new Vector3(characterControl.LookVector.x, characterControl.LookVector.y, 0));
			RaycastInput raycastInput = new RaycastInput
			{
				Start = ray.origin,
				End = ray.origin + ray.direction * 1000f,
				Filter = CollisionFilter.Default
			};

			if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
			{
				float3 hitPoint = hit.Position;

				// Calculate the required rotation
				quaternion targetRotation = TransformHelpers.LookAtRotation(gunTransform.Position, hitPoint, math.up());

				// Apply the rotation to the gun
				gunTransform.Position = transform.Position + new float3(.5f, 2, .25f);
				gunTransform.Rotation = targetRotation;
				SystemAPI.SetComponent(characterComponent.GunPrefabEntity, gunTransform);
			}
		}
	}
}
