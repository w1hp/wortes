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
					SystemAPI.Query<RefRO<PickupMagnet>,
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
						break;
					case StatefulEventState.Stay:
						var otherTransform = SystemAPI.GetComponent<LocalTransform>(otherEntity);
						var direction = transform.ValueRO.Position - otherTransform.Position;

						PhysicsVelocity velocity = new PhysicsVelocity
						{
							Linear = math.lerp(direction, (direction * 10), 0.5f),
							Angular = float3.zero
						};

						ECB.SetComponent(otherEntity, velocity);
						break;
					case StatefulEventState.Exit:
						break;
				}
			}
		}
	}

}
