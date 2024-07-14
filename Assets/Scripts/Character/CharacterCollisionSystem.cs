using Unity.Burst;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

[UpdateAfter(typeof(PickupMagnetSystem))]
partial struct CharacterCollisionSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{

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

		foreach (var (characterComponent, collisionEventBuffer, entity) in
			SystemAPI.Query<RefRO<CharacterComponent>, DynamicBuffer<StatefulKinematicCharacterHit>>()
			.WithEntityAccess())
		{
			for (int i = 0; i < collisionEventBuffer.Length; i++)
			{
				var collisionEvent = collisionEventBuffer[i];
				var otherEntity = collisionEvent.Hit.Entity;

				if (!itemMask.MatchesIgnoreFilter(otherEntity)) continue;

				switch (collisionEvent.State)
				{
					case CharacterHitState.Enter:
						var item = SystemAPI.GetComponent<Item>(otherEntity);
						var inventory = SystemAPI.GetComponent<Inventory>(entity);
						switch (item.Type)
						{
							case Element.Fire:
								inventory.Fire += item.Value;
								break;
							case Element.Water:
								inventory.Water += item.Value;
								break;
							case Element.Earth:
								inventory.Earth += item.Value;
								break;
							case Element.Wood:
								inventory.Wood += item.Value;
								break;
							case Element.Metal:
								inventory.Metal += item.Value;
								break;
						}
						ECB.SetComponent(entity, inventory);


						ECB.DestroyEntity(otherEntity);
						break;
					case CharacterHitState.Stay:
						break;
					case CharacterHitState.Exit:
						break;
				}
			}
		}
	}
}
