using Unity.Entities;
using UnityEngine;

class DropAuthoring : MonoBehaviour
{
    public GameObject ResourcePrefab;
    public int ResourceAmount = 3;
}

class DropAuthoringBaker : Baker<DropAuthoring>
{
    public override void Bake(DropAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new Drop
        {
            ResourcePrefab = GetEntity(authoring.ResourcePrefab, TransformUsageFlags.Dynamic),
            ResourceAmount = authoring.ResourceAmount
        });

        AddComponent(entity, new IsExistTag());
        
    }
}

public struct Drop : IComponentData
{
    public Entity ResourcePrefab;
    public int ResourceAmount;
}
