using Unity.Entities;
using UnityEngine;

class ItemAuthoring : MonoBehaviour
{
	public Element Type;
	public int Value = 1;
}

class ItemAuthoringBaker : Baker<ItemAuthoring>
{
    public override void Bake(ItemAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
		AddComponent(entity, new Item
        {
			Type = authoring.Type,
			Value = authoring.Value
		});
	}
}

public struct Item : IComponentData
{
    public Element Type;
    public int Value;
}
