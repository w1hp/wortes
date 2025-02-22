using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

[UpdateAfter(typeof(DropSystem))]
partial struct PickupMagnetSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PickupMagnet>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var itemQuery = SystemAPI.QueryBuilder()
			.WithAll<Item>()
			.Build();

		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		var itemMask = itemQuery.GetEntityQueryMask();

		foreach (var (pickupMagnet, triggerEventBuffer, transform, entity) in
					SystemAPI.Query<RefRW<PickupMagnet>,
						DynamicBuffer<StatefulTriggerEvent>,
						RefRO<LocalToWorld>>()
					.WithEntityAccess())
		{
			for (int i = 0; i < triggerEventBuffer.Length; i++)
			{
				var triggerEvent = triggerEventBuffer[i];
				var otherEntity = triggerEvent.GetOtherEntity(entity);

				if (!itemMask.MatchesIgnoreFilter(otherEntity)) continue;

				switch (triggerEvent.State)
				{
					case StatefulEventState.Enter:
					case StatefulEventState.Stay:
						var item = SystemAPI.GetComponent<Item>(otherEntity);
						var characterResourcesEntity = pickupMagnet.ValueRW.CharacterResourcesEntity;
						var characterResources = SystemAPI.GetComponent<CharacterResources>(characterResourcesEntity);

						switch (item.Type)
						{
							case ElementType.Fire:
								characterResources.Fire += item.Value;
								break;
							case ElementType.Water:
								characterResources.Water += item.Value;
								break;
							case ElementType.Earth:
								characterResources.Earth += item.Value;
								break;
							case ElementType.Wood:
								characterResources.Wood += item.Value;
								break;
							case ElementType.Metal:
								characterResources.Metal += item.Value;
								break;
						}
						characterResources.Gold++;
						ECB.SetComponent(characterResourcesEntity, characterResources);

						ECB.DestroyEntity(otherEntity);
						break;
					case StatefulEventState.Exit:
						break;
				}
			}
		}
	}

}
