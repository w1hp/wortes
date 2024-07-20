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

}

class HealthAuthoringBaker : Baker<HealthAuthoring>
{
	public override void Bake(HealthAuthoring authoring)
	{
		var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

		AddComponent(entity, new Health
		{
			CurrentHealth = authoring.maxHealth,
			FireResistance = authoring.fireResistance,
			WaterResistance = authoring.waterResistance,
			EarthResistance = authoring.earthResistance,
			WoodResistance = authoring.woodResistance,
			MetalResistance = authoring.metalResistance
		});
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

	public void TakeDamage(float amount, ElementType typeDamage)
	{
		switch (typeDamage)
		{
			case ElementType.Fire:
				CurrentHealth -= amount * (100 - FireResistance) / 100;
				break;
			case ElementType.Water:
				CurrentHealth -= amount * (100 - WaterResistance) / 100;
				break;
			case ElementType.Earth:
				CurrentHealth -= amount * (100 - EarthResistance) / 100;
				break;
			case ElementType.Wood:
				CurrentHealth -= amount * (100 - WoodResistance) / 100;
				break;
			case ElementType.Metal:
				CurrentHealth -= amount * (100 - MetalResistance) / 100;
				break;
			default:
				Debug.Log("Unidentified type dame");
				break;
		}
	}
}
