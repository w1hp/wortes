using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct CleanupAnimateRefSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		var ecb = new EntityCommandBuffer(Allocator.TempJob);

		foreach (var (animatorReference, entity) in
			SystemAPI.Query<AnimatorReference>()
				.WithNone<AnimatedGameObjectPrefab, LocalTransform>()
				.WithEntityAccess())
		{
			Object.Destroy(animatorReference.Value.gameObject);
			ecb.RemoveComponent<AnimatorReference>(entity);
		}

		ecb.Playback(state.EntityManager);
		ecb.Dispose();
	}
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct ConnectGameObjectsSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		var ecb = new EntityCommandBuffer(Allocator.TempJob);

		foreach (var (gameObjectPrefab, entity) in
			SystemAPI.Query<AnimatedGameObjectPrefab>()
				.WithNone<AnimatorReference>()
				.WithEntityAccess())
		{
			var newCompanionGameObject = Object.Instantiate(gameObjectPrefab.Value);

			var newAnimatorReference = new AnimatorReference
			{
				Value = newCompanionGameObject.GetComponent<Animator>()
			};
			newAnimatorReference.Value.SetFloat("Speed", 5f);
			newAnimatorReference.Value.SetFloat("MotionSpeed", 1f);

			ecb.AddComponent(entity, newAnimatorReference);
		}

		ecb.Playback(state.EntityManager);
		ecb.Dispose();
	}
}

[UpdateInGroup(typeof(PresentationSystemGroup))]
partial struct UpdateAnimationsSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		// Update the position and rotation of the game objects

		//foreach (var (transform, animatorReference, moveInput) in
		//SystemAPI.Query<LocalTransform, AnimatorReference, MoveInput>())
		foreach (var (transform, animatorReference) in
		SystemAPI.Query<LocalTransform, AnimatorReference>())
		{
			//animatorReference.Value.SetBool("IsMoving", math.length(moveInput.Value) > 0f);
			var correctPosition = transform.Position;
			correctPosition.y = 0;

			animatorReference.Value.transform.SetPositionAndRotation(correctPosition, math.mul(transform.Rotation, quaternion.RotateY(3.14f)));
		}
	}
}