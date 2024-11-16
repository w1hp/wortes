using Unity.Burst;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

[UpdateAfter(typeof(PickupMagnetSystem))]
partial struct CharacterCollisionSystem : ISystem
{
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
							var characterResources = SystemAPI.GetComponent<CharacterResources>(entity);
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
							ECB.SetComponent(entity, characterResources);

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
							ECB.AddComponent(entity, new DamageToCharacter
							{
								Value = enemy.Damage,
								Type = enemy.EnemyType
							});
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
