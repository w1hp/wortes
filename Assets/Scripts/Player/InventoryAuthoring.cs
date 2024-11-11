using Unity.Entities;
using UnityEngine;

class InventoryAuthoring : MonoBehaviour
{
	public float Level;
	public float Gold;
	public float Wood;
	public float Metal;
	public float Water;
	public float Earth;
	public float Fire;

	class Baker : Baker<InventoryAuthoring>
	{
		public override void Bake(InventoryAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new CharacterResources
			{
				Level = authoring.Level,
				Gold = authoring.Gold,
				Wood = authoring.Wood,
				Metal = authoring.Metal,
				Water = authoring.Water,
				Earth = authoring.Earth,
				Fire = authoring.Fire
			});
		}
	}
}