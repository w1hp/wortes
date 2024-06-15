using Unity.Entities;
using UnityEngine;

class CharacterEquipmentAuthoring : MonoBehaviour
{
	public GameObject Character;
	public GameObject WeaponSlot1;
	public GameObject WeaponSlot2;
	public GameObject WeaponSlot3;
	public GameObject WeaponSlot4;

	public GameObject TowerSlot1;
	public GameObject TowerSlot2;
	public GameObject TowerSlot3;
	public GameObject TowerSlot4;

	class Baker : Baker<CharacterEquipmentAuthoring>
	{
		public override void Bake(CharacterEquipmentAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new CharacterEquipment
			{
				Character = GetEntity(authoring.Character, TransformUsageFlags.Dynamic),
				//WeaponSlot1 = GetEntity(authoring.WeaponSlot1, TransformUsageFlags.Dynamic),
				//WeaponSlot2 = GetEntity(authoring.WeaponSlot2, TransformUsageFlags.Dynamic),
				//WeaponSlot3 = GetEntity(authoring.WeaponSlot3, TransformUsageFlags.Dynamic),
				//WeaponSlot4 = GetEntity(authoring.WeaponSlot4, TransformUsageFlags.Dynamic),

				TowerSlot1 = GetEntity(authoring.TowerSlot1, TransformUsageFlags.Dynamic),
				TowerSlot2 = GetEntity(authoring.TowerSlot2, TransformUsageFlags.Dynamic),
				//TowerSlot3 = GetEntity(authoring.TowerSlot3, TransformUsageFlags.Dynamic),
				//TowerSlot4 = GetEntity(authoring.TowerSlot4, TransformUsageFlags.Dynamic)
			});
		}
	}
}



public struct CharacterEquipment : IComponentData
{
	public Entity Character;
    public Entity SelectedWeapon;
	public Entity WeaponSlot1;
	public Entity WeaponSlot2;
	public Entity WeaponSlot3;
	public Entity WeaponSlot4;

	public Entity SelectedTower;
    public Entity TowerSlot1;
	public Entity TowerSlot2;
	public Entity TowerSlot3;
	public Entity TowerSlot4;
}
