using Unity.Entities;
using UnityEngine;

class BuilderTowerAuthoring : MonoBehaviour
{
    
}

class BuilderTowerAuthoringBaker : Baker<BuilderTowerAuthoring>
{
    public override void Bake(BuilderTowerAuthoring authoring)
    {
		var entity = GetEntity(TransformUsageFlags.Dynamic);

		AddComponent(entity, new BuilderTower
		{

		});

	}
}

public struct BuilderTower : IComponentData
{

}
