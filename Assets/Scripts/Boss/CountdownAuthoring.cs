using Unity.Entities;
using UnityEngine;

class CountdownAuthoring : MonoBehaviour
{
	public float TargetDuration;

	class Baker : Baker<CountdownAuthoring>
	{
		public override void Bake(CountdownAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity, new BossCountdown
			{
				TargetDuration = authoring.TargetDuration,
			});
			AddComponent(entity, new BossInitialization());
		}
	}
}

public struct BossCountdown : IComponentData
{
	public float TargetDuration;
}
public struct BossInitialization : IComponentData
{
    
}