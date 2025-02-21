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
		var selectedTowerCost = SystemAPI.GetComponentRO<TowerCost>(characterEquipment.SelectedTower);
		var selectedTowerElement = SystemAPI.GetComponentRO<TowerElement>(characterEquipment.SelectedTower);



		resourseController.UpdateResourceText(characterResources.Metal, characterResources.Fire,
			characterResources.Water, characterResources.Earth, characterResources.Wood);

		resourseController.UpdateTowerCostText(selectedTowerCost.ValueRO.Cost);
		resourseController.UpdateSelectedTower(selectedTowerElement.ValueRO.Element);
	}
}
