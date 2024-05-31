using Unity.Entities;
using UnityEngine;

class InventoryAuthoring : MonoBehaviour
{
    public uint Gold;
	public uint Wood;
	public uint Metal;
	public uint Water;
	public uint Earth;
	public uint Fire;
}

class InventoryAuthoringBaker : Baker<InventoryAuthoring>
{
    public override void Bake(InventoryAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
		AddComponent(entity, new Inventory
		{
			Gold = authoring.Gold,
			Wood = authoring.Wood,
			Metal = authoring.Metal,
			Water = authoring.Water,
			Earth = authoring.Earth,
			Fire = authoring.Fire
		});
	}
}

public struct Inventory : IComponentData
{
    public uint Gold;
	public uint Wood;
	public uint Metal;
	public uint Water;
	public uint Earth;
	public uint Fire;
}

public enum Element
{
	Unidentified,
	Fire, // ICE
	Water, // LIGHTNING, DESERT
	Earth, // WIND, LIGHTNING
	Wood, 
	Metal 
}