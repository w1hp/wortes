using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class DealDamageSystem : SystemBase
{
	public Action<float, float3> OnDealDamage;
	public Action<float, float3> OnGrantExperience;

	protected override void OnUpdate()
	{
		var ecb = new EntityCommandBuffer(Allocator.Temp);

		// For each character with a damage component...
		foreach (var (health, damageToCharacter, experiencePoints, transform, entity) in
				 SystemAPI.Query<RefRW<Health>, DamageToCharacter, CharacterExperiencePoints,
					 LocalTransform>().WithEntityAccess())
		{
			health.ValueRW.TakeDamage(damageToCharacter.Value, damageToCharacter.Type);
			// Subtract health from the character
			//hitPoints.ValueRW.Value -= damageToCharacter.Value;

			// Invoke the OnDealDamage event, passing in the required data
			OnDealDamage?.Invoke(damageToCharacter.Value, transform.Position);

			ecb.RemoveComponent<DamageToCharacter>(entity);

			// If the damaged character is out of health... Add experience to the player
			if (health.ValueRO.CurrentHealth <= 0f)
			{
				//ecb.DestroyEntity(entity);
				//var originCharacterExperience =
				//	SystemAPI.GetComponent<CharacterExperiencePoints>(damageToCharacter.OriginCharacter);
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
