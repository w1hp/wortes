using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GunAuthoring : MonoBehaviour
{
	public GameObject Barrel;
	class Baker : Baker<GunAuthoring>
	{
		public override void Bake(GunAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new Gun
			{
				Barrel = GetEntity(authoring.Barrel, TransformUsageFlags.Dynamic)
			});
		}
	}
}

public struct Gun : IComponentData
{
	public Entity Barrel;
}