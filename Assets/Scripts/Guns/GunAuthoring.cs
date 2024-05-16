using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GunAuthoring : MonoBehaviour
{
	public GameObject Muzzle;
	public GameObject Bullet;
	class Baker : Baker<GunAuthoring>
	{
		public override void Bake(GunAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new Gun
			{
				Muzzle = GetEntity(authoring.Muzzle, TransformUsageFlags.Dynamic),
				Bullet = GetEntity(authoring.Bullet, TransformUsageFlags.Dynamic)
			});
		}
	}
}

public struct Gun : IComponentData
{
	public Entity Muzzle;
	public Entity Bullet;
}