using Unity.Entities;
using UnityEngine;

class CharacterEquipmentAuthoring : MonoBehaviour
{
	public GameObject Character;

	public GameObject SelectedWeapon;
	public GameObject WeaponSlot1;
	public GameObject WeaponSlot2;
	public GameObject WeaponSlot3;
	public GameObject WeaponSlot4;
	public GameObject WeaponSlot5;

	public GameObject SelectedTower;
	public GameObject FireTower;
	public GameObject WaterTower;
	public GameObject EarthTower;
	public GameObject WoodTower;
	public GameObject MetalTower;

	class Baker : Baker<CharacterEquipmentAuthoring>
	{
		public override void Bake(CharacterEquipmentAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new CharacterEquipment
			{
				Character = GetEntity(authoring.Character, TransformUsageFlags.Dynamic),
				SelectedWeapon = GetEntity(authoring.SelectedWeapon, TransformUsageFlags.Dynamic),
				WeaponSlot1 = GetEntity(authoring.WeaponSlot1, TransformUsageFlags.Dynamic),
				WeaponSlot2 = GetEntity(authoring.WeaponSlot2, TransformUsageFlags.Dynamic),
				WeaponSlot3 = GetEntity(authoring.WeaponSlot3, TransformUsageFlags.Dynamic),
				WeaponSlot4 = GetEntity(authoring.WeaponSlot4, TransformUsageFlags.Dynamic),
				WeaponSlot5 = GetEntity(authoring.WeaponSlot5, TransformUsageFlags.Dynamic),

				SelectedTower = GetEntity(authoring.SelectedTower, TransformUsageFlags.Dynamic),
				FireTower = GetEntity(authoring.FireTower, TransformUsageFlags.Dynamic),
				WaterTower = GetEntity(authoring.WaterTower, TransformUsageFlags.Dynamic),
				EarthTower = GetEntity(authoring.EarthTower, TransformUsageFlags.Dynamic),
				WoodTower = GetEntity(authoring.WoodTower, TransformUsageFlags.Dynamic),
				MetalTower = GetEntity(authoring.MetalTower, TransformUsageFlags.Dynamic),
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
	public Entity WeaponSlot5;

	public Entity SelectedTower;
    public Entity FireTower;
	public Entity WaterTower;
	public Entity EarthTower;
	public Entity WoodTower;
	public Entity MetalTower;
}