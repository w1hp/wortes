using Unity.Entities;
using UnityEngine;

class TowerCollisionAuthoring : MonoBehaviour
{
	class Baker : Baker<TowerCollisionAuthoring>
	{
		public override void Bake(TowerCollisionAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new TowerCollision { CanBuild = true });
		}
	}
}