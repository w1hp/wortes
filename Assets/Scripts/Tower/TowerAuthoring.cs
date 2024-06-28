using Unity.Entities;
using UnityEngine;

class TowerAuthoring : MonoBehaviour
{
	public float GoldCost;
	[Range(0f, 1f)] public float RefundFactor;

	public float WoodCost;
	public float MetalCost;
	public float WaterCost;
	public float EarthCost;
	public float FireCost;

	public float Range;
	public float Damage;
	public float AttackSpeed;
	public float ProjectileSpeed;

	public GameObject ProjectileSpawnPoint;
	public GameObject Aimer;

}

class TowerAuthoringBaker : Baker<TowerAuthoring>
{
    public override void Bake(TowerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
		AddComponent(entity, new Tower
		{
			GoldCost = authoring.GoldCost,
			RefundFactor = authoring.RefundFactor,

			WoodCost = authoring.WoodCost,
			MetalCost = authoring.MetalCost,
			WaterCost = authoring.WaterCost,
			EarthCost = authoring.EarthCost,
			FireCost = authoring.FireCost,
			
			Range = authoring.Range,
			Damage = authoring.Damage,
			AttackSpeed = authoring.AttackSpeed,
			ProjectileSpeed = authoring.ProjectileSpeed,

			ProjectileSpawnPoint = GetEntity(authoring.ProjectileSpawnPoint, TransformUsageFlags.Dynamic),
			Aimer = GetEntity(authoring.Aimer, TransformUsageFlags.Dynamic)
		});
	}
}

public struct Tower : IComponentData
{
	public float GoldCost;
	public float RefundFactor;

	public float WoodCost;
	public float MetalCost;
	public float WaterCost;
	public float EarthCost;
	public float FireCost;

	public float Range;
	public float Damage;
	public float AttackSpeed;
	public float ProjectileSpeed;

	public Entity ProjectileSpawnPoint;
	public Entity Aimer;
}
