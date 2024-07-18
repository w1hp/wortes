using Unity.Burst;
using Unity.Entities;

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
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot1;
                    characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.FireTower;
					break;
                case 1:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot2;
                    characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.WaterTower;
					break;
				case 2:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot3;
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.EarthTower;
					break;
				case 3:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot4;
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.WoodTower;
					break;
				case 4:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot5;
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.MetalTower;
					break;
				default:
                    break;
            }
        }
    }
}
