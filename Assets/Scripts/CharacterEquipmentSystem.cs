using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

partial struct CharacterEquipmentSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CharacterEquipment>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (characterEquipment, entity) in SystemAPI.Query<RefRW<CharacterEquipment>>()
			.WithEntityAccess()
			.WithNone<Prefab>())
		{
			var character = SystemAPI.GetComponentRO<CharacterComponent>(characterEquipment.ValueRO.Character);
			switch (character.ValueRO.CurrentSlot)
			{
				case 0:
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.MetalTower;
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot0, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);
					break;
				case 1:
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.FireTower;
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot0, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);
					break;
				case 2:
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.WaterTower;					
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot0, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);
					break;
				case 3:
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.EarthTower;					
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot0, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);
					break;
				case 4:
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.WoodTower;					
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot0, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, true);
					break;
				default:
					break;
			}
		}
	}
}
