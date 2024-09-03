using UnityEngine;
using Unity.Entities;

public class IsNotPauseAuthoring : MonoBehaviour
{
    class IsNotPauseBaker : Baker<IsNotPauseAuthoring>
	{
		public override void Bake(IsNotPauseAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity, new IsNotPause());
		}
	}
}

public struct IsNotPause : IComponentData, IEnableableComponent
{
    
}
