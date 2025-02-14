using Unity.Burst;
using Unity.Entities;
using UnityEngine.TextCore.Text;

partial struct ResourceUpdateSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CharacterResources>();
	}

	public void OnUpdate(ref SystemState state)
	{
		if (ResourceController.Singleton == null)
			return;
		var resourseController = ResourceController.Singleton;

		Entity character;
		if (!SystemAPI.TryGetSingletonEntity<CharacterResources>(out character))
			return;

		CharacterResources characterResources = SystemAPI.GetComponentRO<CharacterResources>(character).ValueRO;
		CharacterEquipment characterEquipment = SystemAPI.GetComponentRO<CharacterEquipment>(character).ValueRO;
		RefRO<TowerCost> metalTowerCost = SystemAPI.GetComponentRO<TowerCost>(characterEquipment.MetalTower);
		RefRO<TowerCost> fireTowerCost = SystemAPI.GetComponentRO<TowerCost>(characterEquipment.FireTower);
		RefRO<TowerCost> waterTowerCost = SystemAPI.GetComponentRO<TowerCost>(characterEquipment.WaterTower);
		RefRO<TowerCost> earthTowerCost = SystemAPI.GetComponentRO<TowerCost>(characterEquipment.EarthTower);
		RefRO<TowerCost> woodTowerCost = SystemAPI.GetComponentRO<TowerCost>(characterEquipment.WoodTower);
		
		resourseController.UpdateResourceText(characterResources.Wood, characterResources.Fire,
			characterResources.Water, characterResources.Earth, characterResources.Metal);

		resourseController.UpdateTowerCostText(metalTowerCost.ValueRO.Cost,
			fireTowerCost.ValueRO.Cost, waterTowerCost.ValueRO.Cost,
			earthTowerCost.ValueRO.Cost, woodTowerCost.ValueRO.Cost);
	}
}
