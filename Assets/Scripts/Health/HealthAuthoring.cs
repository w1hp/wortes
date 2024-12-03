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

public struct Health : IComponentData
{
	public float CurrentHealth;
	public float MaxHealth;
	public float FireResistance;
	public float WaterResistance;
	public float EarthResistance;
	public float WoodResistance;
	public float MetalResistance;

	public bool TakeDamage(float amount, ElementType typeDamage)
	{
		var isHealing = false;
		var tmpHealth = CurrentHealth;
		switch (typeDamage)
		{
			case ElementType.Fire:
				tmpHealth -= amount * (100 - FireResistance) / 100;
				isHealing = tmpHealth >= CurrentHealth ? true : false;
				CurrentHealth = tmpHealth;
				return isHealing;
			case ElementType.Water:
				tmpHealth -= amount * (100 - WaterResistance) / 100;
				isHealing = tmpHealth >= CurrentHealth ? true : false;
				CurrentHealth = tmpHealth;
				return isHealing;
			case ElementType.Earth:
				tmpHealth -= amount * (100 - EarthResistance) / 100;
				isHealing = tmpHealth >= CurrentHealth ? true : false;
				CurrentHealth = tmpHealth;
				return isHealing;
			case ElementType.Wood:
				tmpHealth -= amount * (100 - WoodResistance) / 100;
				isHealing = tmpHealth >= CurrentHealth ? true : false;
				CurrentHealth = tmpHealth;
				return isHealing;
			case ElementType.Metal:
				tmpHealth -= amount * (100 - MetalResistance) / 100;
				isHealing = tmpHealth >= CurrentHealth ? true : false;
				CurrentHealth = tmpHealth;
				return isHealing;
			default:
#if UNITY_EDITOR
				Debug.Log("Unidentified type dame");
#endif
				return isHealing;
		}
	}
}
