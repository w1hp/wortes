using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class DealDamageSystem : SystemBase
{
	public Action<float, float3, bool> OnDealDamage;
	//public Action<float, float3> OnGrantExperience;

	protected override void OnUpdate()
	{
		var ecb = new EntityCommandBuffer(Allocator.Temp);

		// For each character with a damage component...
		foreach (var (health, damageToCharacter, transform, entity) in
				 SystemAPI.Query<RefRW<Health>, RefRO<DamageToCharacter>, LocalTransform>().WithEntityAccess())
		{
			var isHealing = health.ValueRW.TakeDamage(damageToCharacter.ValueRO.Value, damageToCharacter.ValueRO.Type);
			// Subtract health from the character
			//hitPoints.ValueRW.Value -= damageToCharacter.Value;

			// Invoke the OnDealDamage event, passing in the required data
			OnDealDamage?.Invoke(damageToCharacter.ValueRO.Value, transform.Position, isHealing);

			ecb.RemoveComponent<DamageToCharacter>(entity);

			// If the damaged character is out of health... Add experience to the player
			if (health.ValueRO.CurrentHealth <= 0f)
			{
				var killStatistics = SystemAPI.GetSingletonRW<KillStatistics>();
				switch (damageToCharacter.ValueRO.OriginCharacterType)
				{
					case OriginCharacterType.Enemy:
						return;
					case OriginCharacterType.Player:
						killStatistics.ValueRW.EnemyCount++;
						killStatistics.ValueRW.PlayerFragCount++;
						break;
					case OriginCharacterType.Tower:
						killStatistics.ValueRW.EnemyCount++;
						killStatistics.ValueRW.TowerFragCount++;
						break;
				}
				//var originCharacterExperience = SystemAPI.GetComponent<CharacterExperiencePoints>(damageToCharacter.OriginCharacter);
				//originCharacterExperience.Value += experiencePoints.Value;
				//SystemAPI.SetComponent(damageToCharacter.OriginCharacter, originCharacterExperience);

				//var originCharacterPosition =
				//	SystemAPI.GetComponent<LocalTransform>(damageToCharacter.OriginCharacter).Position;
				//OnGrantExperience?.Invoke(experiencePoints.Value, originCharacterPosition);
				ecb.SetComponentEnabled<IsExistTag>(entity, false);
			}
		}

		ecb.Playback(EntityManager);
		ecb.Dispose();
	}
}
