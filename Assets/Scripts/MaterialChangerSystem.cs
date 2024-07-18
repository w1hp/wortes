using Unity.Entities;
using Unity.Rendering;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Physics.Stateful;
using Unity.Assertions;

using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct MaterialChangerSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<MaterialChanger>();
	}
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var nonTriggerQuery = SystemAPI.QueryBuilder().WithNone<StatefulTriggerEvent>().Build();
		Assert.IsFalse(nonTriggerQuery.HasFilter(),
			"The use of EntityQueryMask in this system will not respect the query's active filter settings.");
		var nonTriggerMask = nonTriggerQuery.GetEntityQueryMask();

		var materialMeshInfoLookup = SystemAPI.GetComponentLookup<MaterialMeshInfo>();

		foreach (var (triggerEventBuffer, materialChanger, entity) in
			SystemAPI.Query<
				DynamicBuffer<StatefulTriggerEvent>,
				RefRW<MaterialChanger>>()
				.WithEntityAccess())
		//,			RefRW<URPMaterialPropertyBaseColor> baseColor
		{
			var characterEQ = SystemAPI.GetComponentRO<CharacterEquipment>(materialChanger.ValueRO.Character);
			var character = SystemAPI.GetComponentRO<CharacterComponent>(materialChanger.ValueRO.Character);
			//var highlighterMMI = SystemAPI.GetComponentRW<MaterialMeshInfo>(entity);
			//var highlighterMMI = materialMeshInfoLookup[entity];

			var highlighterChildren = SystemAPI.GetBuffer<Child>(entity);
			var graphicEntity = highlighterChildren[0].Value;
			var baseColor = SystemAPI.GetComponentRW<URPMaterialPropertyBaseColor>(graphicEntity);
			//var graphicEntity = SystemAPI.GetComponentRO<GraphicEntity>(entity);

			materialChanger.ValueRW.BuildTime -= SystemAPI.Time.DeltaTime;

			if (!character.ValueRO.IsBuildMode)
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(graphicEntity, false);
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.GunPrefabEntity, true);
				break;
			}

			//var selectedTowerMMI = materialMeshInfoLookup[characterEQ.ValueRO.SelectedTower];
			//var selectedTowerMMI = SystemAPI.GetComponentRO<MaterialMeshInfo>(characterEQ.ValueRO.SelectedTower);
			//highlighterMMI.ValueRW.Mesh = selectedTowerMMI.ValueRO.Mesh;

			SystemAPI.SetComponentEnabled<MaterialMeshInfo>(graphicEntity, true);
			SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.GunPrefabEntity, false);

			for (int i = 0; i < triggerEventBuffer.Length; i++)
			{
				var triggerEvent = triggerEventBuffer[i];
				var otherEntity = triggerEvent.GetOtherEntity(entity);

				// exclude other triggers and processed events
				if (triggerEvent.State == StatefulEventState.Stay ||
						!nonTriggerMask.MatchesIgnoreFilter(otherEntity))
				{
					continue;
				}
				if (triggerEvent.State == StatefulEventState.Enter)
				{
					materialChanger.ValueRW.CanBuild = false;
					//RED
					baseColor.ValueRW.Value = new float4(1, 0, 0, 0.5f);
				}
				else if (triggerEvent.State == StatefulEventState.Exit)
				{
					materialChanger.ValueRW.CanBuild = true;
					//GREEN
					baseColor.ValueRW.Value = new float4(0, 1, 0, 0.5f);
				}
			}
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
				else Debug.Log("Not enough resources");


			}

		}

	}
}