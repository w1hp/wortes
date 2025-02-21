using Unity.Entities;
using UnityEngine;

class SfxComponentAuthoring : MonoBehaviour
{

	class Baker : Baker<SfxComponentAuthoring>
	{
		public override void Bake(SfxComponentAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new SfxComponent
			{
			});
		}
	}
}

public struct SfxComponent : IComponentData
{

}
