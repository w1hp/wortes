using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(CustomUpdateGroup))]
public partial class DealDamageSystem : SystemBase
{
	public Action<float, float3, AttackResult> OnDealDamage;
	//public Action<float, float3> OnGrantExperience;

	protected override void OnUpdate()
	{
		var ecb = new EntityCommandBuffer(Allocator.Temp);

		foreach (var (health, damageToCharacter, transform, entity) in
				 SystemAPI.Query<RefRW<Health>, RefRO<DamageToCharacter>, LocalTransform>().WithEntityAccess())
		{
			var atackResult = health.ValueRW.TakeDamage(damageToCharacter.ValueRO.Value, damageToCharacter.ValueRO.Type);

			OnDealDamage?.Invoke(damageToCharacter.ValueRO.Value, transform.Position, atackResult);

			ecb.RemoveComponent<DamageToCharacter>(entity);

			if (health.ValueRO.CurrentHealth <= 0f)
			{
				var killStatistics = SystemAPI.GetSingletonRW<KillStatistics>();
				switch (damageToCharacter.ValueRO.OriginCharacterType)
				{
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
