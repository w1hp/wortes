using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
partial struct UpdateVfxSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		//		foreach (var (localToWorld, vfxComponent, entity) in
		//			SystemAPI.Query<RefRO<LocalToWorld>, RefRW<VfxComponent>>().WithEntityAccess())
		//		{
		////			float3 offset = float3.zero;

		////			VfxPool.Instance.UpdateVfx(
		////				localToWorld.ValueRO.Position + offset,
		////				localToWorld.ValueRO.Rotation,
		////				entity);

		////			if (vfxComponent.ValueRO.VfxType == VfxType.OneShot)
		////			{
		////				vfxComponent.ValueRW.Time -= SystemAPI.Time.DeltaTime;
		//////				if (vfxComponent.ValueRW.Time < 0)
		//////				{
		//////#if UNITY_EDITOR
		//////					UnityEngine.Debug.Log("Bu");
		//////#endif
		//////					VfxPool.Instance.RemoveVfx(entity);
		//////					state.EntityManager.SetComponentEnabled<VfxComponent>(entity, false);
		//////				}
		////			}
		//		}
		foreach (var (particleSystem, audioSource, entity) in
			SystemAPI.Query<SystemAPI.ManagedAPI.UnityEngineComponent<UnityEngine.ParticleSystem>,
				SystemAPI.ManagedAPI.UnityEngineComponent<UnityEngine.AudioSource>>()
				.WithAll<VfxComponent>()
				.WithEntityAccess())
		{
			particleSystem.Value.Play(true);
			audioSource.Value.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
			audioSource.Value.Play();
			state.EntityManager.SetComponentEnabled<VfxComponent>(entity, false);
		}

		if (VfxPool.Instance != null)
			VfxPool.Instance.ClearMissingEntities(state.EntityManager);
	}
}
