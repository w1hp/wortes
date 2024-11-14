using Unity.Entities;
using UnityEngine;

class BossAuthoring : MonoBehaviour
{
	class Baker : Baker<BossAuthoring>
	{
		public override void Bake(BossAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<BossTag>(entity);
			AddComponent<BossStateMachine>(entity);
		}
	}
}

public struct BossTag : IComponentData { }

