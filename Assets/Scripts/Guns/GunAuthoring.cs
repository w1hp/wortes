using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GunAuthoring : MonoBehaviour
{
	public GameObject Muzzle;
	public GameObject Bullet;
	public OriginCharacterType OriginCharacterType;
	public GameObject Owner;
	public ElementType ElementType;
	public float Damage;
	public float Strength;
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
				OriginCharacterType = authoring.OriginCharacterType,
				Owner = GetEntity(authoring.Owner, TransformUsageFlags.Dynamic),
				ElementType = authoring.ElementType,
				Damage = authoring.Damage,
				Strength = authoring.Strength,
				FireInterval = authoring.FireInterval
			});
		}
	}
}

public struct Gun : IComponentData, IEnableableComponent
{
	public Entity Muzzle;
	public Entity Bullet;
	public OriginCharacterType OriginCharacterType;
	public Entity Owner;
	public ElementType ElementType;
	public float Damage;
	public float Strength;
	public float FireInterval;
	public float LastShotTime;
}

public enum OriginCharacterType
{
	Unidentified,
	Player,
	Tower,
	Enemy
}
