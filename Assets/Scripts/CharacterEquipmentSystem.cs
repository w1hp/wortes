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
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot1;
                    characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.FireTower;
					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot1, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, true);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot4, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot5, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot5, false);
					break;
                case 1:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot2;
                    characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.WaterTower;
					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot2, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, true);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot4, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot5, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot5, false);
					break;
				case 2:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot3;
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.EarthTower;

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot3, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, true);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot4, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot5, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot5, false);
					break;
				case 3:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot4;
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.WoodTower;

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot4, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, true);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot5, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot5, false);
					break;
				case 4:
					//characterEquipment.ValueRW.SelectedWeapon = characterEquipment.ValueRW.WeaponSlot5;
					characterEquipment.ValueRW.SelectedTower = characterEquipment.ValueRW.MetalTower;

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot1, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot1, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot2, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot2, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot3, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot3, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot4, false);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot4, false);

					// SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRW.WeaponSlot5, true);
					SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WeaponSlot5, true);
					break;
				default:
                    break;
            }
        }
    }
}
