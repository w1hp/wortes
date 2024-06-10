using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GunAuthoring : MonoBehaviour
{
	public GameObject Muzzle;
	public GameObject Bullet;
	public GunOwner OwnerType;
	public GameObject Owner;
	public float FireInterval;
	class Baker : Baker<GunAuthoring>
	{
		public override void Bake(GunAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new Gun
			{
				Muzzle = GetEntity(authoring.Muzzle, TransformUsageFlags.Dynamic),
				Bullet = GetEntity(authoring.Bullet, TransformUsageFlags.Dynamic),
				OwnerType = authoring.OwnerType,
				Owner = GetEntity(authoring.Owner, TransformUsageFlags.Dynamic),
				FireInterval = authoring.FireInterval
			});
		}
	}
}

public struct Gun : IComponentData
{
	public Entity Muzzle;
	public Entity Bullet;
	public GunOwner OwnerType;
	public Entity Owner;
	public float FireInterval;
}

public enum GunOwner
{
	Player,
	Tower,
	Enemy
}
