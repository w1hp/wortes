using Unity.Entities;
using UnityEngine;

class PlayerTagAuthoring : MonoBehaviour
{
	class Baker : Baker<PlayerTagAuthoring>
	{
		public override void Bake(PlayerTagAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new PlayerTag());
			AddComponent(entity, new IsExistTag());
		}
	}
}