using Unity.Entities;
using UnityEngine;


public class AnimatorReferenceAuthoring : MonoBehaviour
{
	public GameObject AnimatedGameObjectPrefab;

	public class Baker : Baker<AnimatorReferenceAuthoring>
	{
		public override void Bake(AnimatorReferenceAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);

			AddComponentObject(entity, new AnimatedGameObjectPrefab
			{
				Value = authoring.AnimatedGameObjectPrefab
			});
		}
	}
}
