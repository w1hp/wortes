using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
	public GameObject CannonBallPrefab;

	class Baker : Baker<ConfigAuthoring>
	{
		public override void Bake(ConfigAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.None);
			AddComponent(entity, new Config
			{
				CannonBallPrefab = GetEntity(authoring.CannonBallPrefab, TransformUsageFlags.Dynamic),
			});
		}
	}
}
public struct Config : IComponentData
{
	public Entity CannonBallPrefab;
}
