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

		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

		var itemLookup = SystemAPI.GetComponentLookup<Item>(true);
		var enemyLookup = SystemAPI.GetComponentLookup<EnemyComponent>(true);


		foreach (var (characterComponent, collisionEventBuffer, entity) in
			SystemAPI.Query<RefRO<CharacterComponent>, DynamicBuffer<StatefulKinematicCharacterHit>>()
			.WithEntityAccess())
		{
			for (int i = 0; i < collisionEventBuffer.Length; i++)
			{
				var collisionEvent = collisionEventBuffer[i];
				var otherEntity = collisionEvent.Hit.Entity;


				if (itemLookup.TryGetComponent(otherEntity, out Item item))
				{
					switch (collisionEvent.State)
					{
						case CharacterHitState.Enter:
							var inventory = SystemAPI.GetComponent<Inventory>(entity);
							switch (item.Type)
							{
								case ElementType.Fire:
									inventory.Fire += item.Value;
									break;
								case ElementType.Water:
									inventory.Water += item.Value;
									break;
								case ElementType.Earth:
									inventory.Earth += item.Value;
									break;
								case ElementType.Wood:
									inventory.Wood += item.Value;
									break;
								case ElementType.Metal:
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
					continue;
				}
				else if (enemyLookup.TryGetComponent(otherEntity, out EnemyComponent enemy))
				{
					switch (collisionEvent.State)
					{
						case CharacterHitState.Enter:
							var health = SystemAPI.GetComponentRW<Health>(entity);
							health.ValueRW.TakeDamage(enemy.Damage, enemy.EnemyType);
							if (health.ValueRW.CurrentHealth <= 0f)
							{
								Debug.Log("Character is dead");
							}
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
}
