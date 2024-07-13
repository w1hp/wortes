using Unity.Entities;
using UnityEngine;

class PickupMagnetAuthoring : MonoBehaviour
{
    public float MagnetRange = 3f;
}

class PickupMagnetAuthoringBaker : Baker<PickupMagnetAuthoring>
{
    public override void Bake(PickupMagnetAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new PickupMagnet
        {
            MagnetRange = authoring.MagnetRange
        });
        
    }
}

public struct PickupMagnet : IComponentData
{
    public float MagnetRange;
}
