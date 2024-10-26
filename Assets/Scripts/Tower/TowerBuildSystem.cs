using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

partial struct TowerBuildSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (triggerEventBuffer, materialChanger, entity) in
	SystemAPI.Query<
		DynamicBuffer<StatefulTriggerEvent>,
		RefRW<MaterialChanger>>()
		.WithEntityAccess())
		{
			RefRO<CharacterEquipment> characterEQ = SystemAPI.GetComponentRO<CharacterEquipment>(materialChanger.ValueRO.Character);
			RefRO<CharacterComponent> character = SystemAPI.GetComponentRO<CharacterComponent>(materialChanger.ValueRO.Character);

			materialChanger.ValueRW.BuildTime -= SystemAPI.Time.DeltaTime;


			if (character.ValueRO.IsBuilding && materialChanger.ValueRO.CanBuild)
			{
				if (materialChanger.ValueRO.BuildTime > 0) return;

				var selectedTowerComponent = SystemAPI.GetComponentRO<Tower>(characterEQ.ValueRO.SelectedTower);
				var characterInventory = SystemAPI.GetComponentRW<Inventory>(materialChanger.ValueRO.Character);
				if (selectedTowerComponent.ValueRO.GoldCost <= characterInventory.ValueRW.Gold &&
					selectedTowerComponent.ValueRO.WoodCost <= characterInventory.ValueRW.Wood &&
					selectedTowerComponent.ValueRO.MetalCost <= characterInventory.ValueRW.Metal &&
					selectedTowerComponent.ValueRO.WaterCost <= characterInventory.ValueRW.Water &&
					selectedTowerComponent.ValueRO.EarthCost <= characterInventory.ValueRW.Earth &&
					selectedTowerComponent.ValueRO.FireCost <= characterInventory.ValueRW.Fire)
				{
					materialChanger.ValueRW.BuildTime = materialChanger.ValueRO.BuildTimeRemaining;
					characterInventory.ValueRW.Gold -= selectedTowerComponent.ValueRO.GoldCost;
					characterInventory.ValueRW.Wood -= selectedTowerComponent.ValueRO.WoodCost;
					characterInventory.ValueRW.Metal -= selectedTowerComponent.ValueRO.MetalCost;
					characterInventory.ValueRW.Water -= selectedTowerComponent.ValueRO.WaterCost;
					characterInventory.ValueRW.Earth -= selectedTowerComponent.ValueRO.EarthCost;
					characterInventory.ValueRW.Fire -= selectedTowerComponent.ValueRO.FireCost;

					Entity towerEntity = state.EntityManager.Instantiate(characterEQ.ValueRO.SelectedTower);
					var towerTransform = state.EntityManager.GetComponentData<LocalTransform>(characterEQ.ValueRO.SelectedTower);
					var highlighterTransform = state.EntityManager.GetComponentData<LocalToWorld>(entity);
					towerTransform.Position = highlighterTransform.Position;
					state.EntityManager.SetComponentData(towerEntity, towerTransform);
				}
#if UNITY_EDITOR
				else Debug.Log("Not enough resources");
#endif
			}
		}
	}
}
