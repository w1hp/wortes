using Unity.Entities;
using UnityEngine;

class CharacterEquipmentAuthoring : MonoBehaviour
{
	public GameObject Character;

	public GameObject SelectedWeapon;
	public GameObject FireGun;
	public GameObject WaterGun;
	public GameObject EarthGun;
	public GameObject WoodGun;
	public GameObject MetalGun;


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

				SelectedWeapon = GetEntity(authoring.SelectedWeapon, TransformUsageFlags.WorldSpace),
				FireGun = GetEntity(authoring.FireGun, TransformUsageFlags.WorldSpace),
				WaterGun = GetEntity(authoring.WaterGun, TransformUsageFlags.WorldSpace),
				EarthGun = GetEntity(authoring.EarthGun, TransformUsageFlags.WorldSpace),
				WoodGun = GetEntity(authoring.WoodGun, TransformUsageFlags.WorldSpace),
				MetalGun = GetEntity(authoring.MetalGun, TransformUsageFlags.WorldSpace),

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
	public Entity FireGun;
	public Entity WaterGun;
	public Entity EarthGun;
	public Entity WoodGun;
	public Entity MetalGun;


	public Entity SelectedTower;
    public Entity FireTower;
	public Entity WaterTower;
	public Entity EarthTower;
	public Entity WoodTower;
	public Entity MetalTower;
}