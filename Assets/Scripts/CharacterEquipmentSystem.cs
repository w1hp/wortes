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
			UpdateCharacterEquipment(characterEquipment, character.ValueRO.CurrentSlot, ref state);
		}
	}

	private void UpdateCharacterEquipment(RefRW<CharacterEquipment> characterEquipment, int currentSlot, ref SystemState state)
	{
		switch (currentSlot)
		{
			case 1:
				SetEquipment(characterEquipment, characterEquipment.ValueRW.MetalTower, characterEquipment.ValueRW.MetalGun, characterEquipment.ValueRO.MetalGun, ref state);
				break;
			case 2:
				SetEquipment(characterEquipment, characterEquipment.ValueRW.FireTower, characterEquipment.ValueRW.FireGun, characterEquipment.ValueRO.FireGun, ref state);
				break;
			case 3:
				SetEquipment(characterEquipment, characterEquipment.ValueRW.WaterTower, characterEquipment.ValueRW.WaterGun, characterEquipment.ValueRO.WaterGun, ref state);
				break;
			case 4:
				SetEquipment(characterEquipment, characterEquipment.ValueRW.EarthTower, characterEquipment.ValueRW.EarthGun, characterEquipment.ValueRO.EarthGun, ref state);
				break;
			case 5:
				SetEquipment(characterEquipment, characterEquipment.ValueRW.WoodTower, characterEquipment.ValueRW.WoodGun, characterEquipment.ValueRO.WoodGun, ref state);
				break;
			default:
				break;
		}
	}

	private void SetEquipment(RefRW<CharacterEquipment> characterEquipment, Entity selectedTower, Entity enabledGun, Entity enabledMesh, ref SystemState state)
	{
		characterEquipment.ValueRW.SelectedTower = selectedTower;
		characterEquipment.ValueRW.SelectedWeapon = enabledGun;

		SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.FireGun, enabledGun == characterEquipment.ValueRW.FireGun);
		SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WaterGun, enabledGun == characterEquipment.ValueRW.WaterGun);
		SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.EarthGun, enabledGun == characterEquipment.ValueRW.EarthGun);
		SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.WoodGun, enabledGun == characterEquipment.ValueRW.WoodGun);
		SystemAPI.SetComponentEnabled<Gun>(characterEquipment.ValueRW.MetalGun, enabledGun == characterEquipment.ValueRW.MetalGun);

		SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRO.FireGun, enabledMesh == characterEquipment.ValueRO.FireGun);
		SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRO.WaterGun, enabledMesh == characterEquipment.ValueRO.WaterGun);
		SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRO.EarthGun, enabledMesh == characterEquipment.ValueRO.EarthGun);
		SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRO.WoodGun, enabledMesh == characterEquipment.ValueRO.WoodGun);
		SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEquipment.ValueRO.MetalGun, enabledMesh == characterEquipment.ValueRO.MetalGun);
	}
}
