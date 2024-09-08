using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class IsometricCameraAuthoring : MonoBehaviour
{
	class Baker : Baker<IsometricCameraAuthoring>
	{
		public override void Bake(IsometricCameraAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);

			AddComponent(entity, new IsometricCamera());
		}
	}
}

public struct IsometricCamera : IComponentData
{

}


