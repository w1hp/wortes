using Unity.Entities;
using UnityEngine;

class HealthAuthoring : MonoBehaviour
{
	public float maxHealth = 100f;
	public float fireResistance = 0f;
	public float waterResistance = 0f;
	public float earthResistance = 0f;
	public float woodResistance = 0f;
	public float metalResistance = 0f;
	class Baker : Baker<HealthAuthoring>
	{
		public override void Bake(HealthAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			AddComponent(entity, new Health
			{
				CurrentHealth = authoring.maxHealth,
				MaxHealth = authoring.maxHealth,
				FireResistance = authoring.fireResistance,
				WaterResistance = authoring.waterResistance,
				EarthResistance = authoring.earthResistance,
				WoodResistance = authoring.woodResistance,
				MetalResistance = authoring.metalResistance
			});
		}
	}
}

public enum AttackResult
{
	Healed,
	Damaged,
	NoEffect
}

public struct Health : IComponentData
{
	public float CurrentHealth;
	public float MaxHealth;
	public float FireResistance;
	public float WaterResistance;
	public float EarthResistance;
	public float WoodResistance;
	public float MetalResistance;

	public (AttackResult, float) TakeDamage(float amount, ElementType typeDamage)
	{
		float tmpHealth = CurrentHealth;
		float resistance = 0f;

		switch (typeDamage)
		{
			case ElementType.Fire:
				resistance = FireResistance;
				break;
			case ElementType.Water:
				resistance = WaterResistance;
				break;
			case ElementType.Earth:
				resistance = EarthResistance;
				break;
			case ElementType.Wood:
				resistance = WoodResistance;
				break;
			case ElementType.Metal:
				resistance = MetalResistance;
				break;
			default:
#if UNITY_EDITOR
				Debug.Log("Unidentified type damage");
#endif
				return (AttackResult.NoEffect, 0f);
		}

		if (resistance == 100f)
		{
			return (AttackResult.NoEffect, 0f);
		}
		var damage = amount * (100 - resistance) / 100;
		tmpHealth -= damage;
		var isHealing = tmpHealth >= CurrentHealth;
		CurrentHealth = tmpHealth;
		var result = isHealing ? AttackResult.Healed : AttackResult.Damaged;
		return (result, damage);
	}
}
