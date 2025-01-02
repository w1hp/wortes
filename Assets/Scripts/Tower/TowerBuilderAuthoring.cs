using Unity.Entities;
using UnityEngine;

class TowerBuilderAuthoring : MonoBehaviour
{
	public float BuildTimeRemaining;

	class Baker : Baker<TowerBuilderAuthoring>
	{
		public override void Bake(TowerBuilderAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new TowerBuildTime
			{
				BuildTimeRemaining = authoring.BuildTimeRemaining,
			});
			AddComponent<TowerBuiltCount>(entity);
			//AddComponent<TowerBuildTime>(entity);
		}
	}
}