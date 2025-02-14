using Unity.Entities;
using UnityEngine;

public class VfxComponentAuthoring : MonoBehaviour
{
	public VfxType VfxType;
	public float Time;
	class Baker : Baker<VfxComponentAuthoring>
	{
		public override void Bake(VfxComponentAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new VfxComponent
			{
				VfxType = authoring.VfxType,
				Time = authoring.Time
			});
			SetComponentEnabled<VfxComponent>(entity, false);
		}
	}
}
public struct VfxComponent : IComponentData, IEnableableComponent
{
	public VfxType VfxType;
	public float Time;
}

public enum VfxType
{
	Looped,
	OneShot
}


