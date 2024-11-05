using Unity.Entities;
using UnityEngine;

class TowerAuthoring : MonoBehaviour
{
	public ElementType Element;
	public float Cost;
	public float Range;
	public GameObject Aimer;
	public ushort Ammo;

	class Baker : Baker<TowerAuthoring>
	{
		public override void Bake(TowerAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			
			AddComponent<TowerTag>(entity);
			
			AddComponent(entity, new TowerElement
			{
				Element = authoring.Element
			});
			
			AddComponent(entity, new TowerCost
			{
				Cost = authoring.Cost
			});

			AddComponent(entity, new TowerAimer
			{
				Range = authoring.Range,
				Aimer = GetEntity(authoring.Aimer, TransformUsageFlags.Dynamic)
			});

			AddComponent(entity, new TowerAmmo
			{
				Ammo = authoring.Ammo
			});
		}
	}
}
