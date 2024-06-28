using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
	public GameObject GraphicsRepresentation;
	class Baker : Baker<ProjectileAuthoring>
	{
		public override void Bake(ProjectileAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			//AddComponent<Projectile>(entity);
			AddComponent(entity, new Projectile
			{
				GraphicsRepresentation = GetEntity(authoring.GraphicsRepresentation, TransformUsageFlags.Dynamic),
			});
		}
	}
}

public struct Projectile : IComponentData
{
	public Entity GraphicsRepresentation;
	public float3 Velocity;
	public float Damage;
    public int Damage;
}
