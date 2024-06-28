using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
    
}

class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
		var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

		AddComponent<EnemyTag>(entity);
	}
}

public struct EnemyTag : IComponentData
{

}
