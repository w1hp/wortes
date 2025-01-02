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
		foreach (var ( towerBuildCount, towerBuildTime, entity) in
			SystemAPI.Query<RefRW<TowerBuiltCount>, RefRW<TowerBuildTime>>()
			.WithEntityAccess())
		{
			towerBuildTime.ValueRW.BuildTime -= SystemAPI.Time.DeltaTime;

			RefRO<CharacterComponent> character = SystemAPI.GetComponentRO<CharacterComponent>(entity);

			if (character.ValueRO.IsBuilding)
			{
				if (towerBuildTime.ValueRO.BuildTime > 0) return;

				RefRO<CharacterEquipment> characterEQ = SystemAPI.GetComponentRO<CharacterEquipment>(entity);
				RefRO<TowerElement> towerElement = SystemAPI.GetComponentRO<TowerElement>(characterEQ.ValueRO.SelectedTower);
				RefRO<TowerCost> towerCost = SystemAPI.GetComponentRO<TowerCost>(characterEQ.ValueRO.SelectedTower);
				RefRW<CharacterResources> characterResources = SystemAPI.GetComponentRW<CharacterResources>(entity);

				switch (towerElement.ValueRO.Element)
				{
					case ElementType.Wood:
						if (towerCost.ValueRO.Cost <= characterResources.ValueRW.Wood)
						{
							characterResources.ValueRW.Wood -= towerCost.ValueRO.Cost;
							towerBuildTime.ValueRW.BuildTime = towerBuildTime.ValueRO.BuildTimeRemaining;
							BuildTower(characterEQ.ValueRO.SelectedTower, entity, ref state);
							towerBuildCount.ValueRW.Count++;
						}
#if UNITY_EDITOR
						else
							Debug.Log("Not enough resources to build tower");
#endif
						break;
					case ElementType.Metal:
						if (towerCost.ValueRO.Cost <= characterResources.ValueRW.Metal)
						{
							characterResources.ValueRW.Metal -= towerCost.ValueRO.Cost;
							towerBuildTime.ValueRW.BuildTime = towerBuildTime.ValueRO.BuildTimeRemaining;
							BuildTower(characterEQ.ValueRO.SelectedTower, entity, ref state);
							towerBuildCount.ValueRW.Count++;
						}
#if UNITY_EDITOR
						else
							Debug.Log("Not enough resources to build tower");
#endif
						break;
					case ElementType.Water:
						if (towerCost.ValueRO.Cost <= characterResources.ValueRW.Water)
						{
							characterResources.ValueRW.Water -= towerCost.ValueRO.Cost;
							towerBuildTime.ValueRW.BuildTime = towerBuildTime.ValueRO.BuildTimeRemaining;
							BuildTower(characterEQ.ValueRO.SelectedTower, entity, ref state);
							towerBuildCount.ValueRW.Count++;
						}
#if UNITY_EDITOR
						else
							Debug.Log("Not enough resources to build tower");
#endif
						break;
					case ElementType.Earth:
						if (towerCost.ValueRO.Cost <= characterResources.ValueRW.Earth)
						{
							characterResources.ValueRW.Earth -= towerCost.ValueRO.Cost;
							towerBuildTime.ValueRW.BuildTime = towerBuildTime.ValueRO.BuildTimeRemaining;
							BuildTower(characterEQ.ValueRO.SelectedTower, entity, ref state);
							towerBuildCount.ValueRW.Count++;
						}
#if UNITY_EDITOR
						else
							Debug.Log("Not enough resources to build tower");
#endif
						break;
					case ElementType.Fire:
						if (towerCost.ValueRO.Cost <= characterResources.ValueRW.Fire)
						{
							characterResources.ValueRW.Fire -= towerCost.ValueRO.Cost;
							towerBuildTime.ValueRW.BuildTime = towerBuildTime.ValueRO.BuildTimeRemaining;
							BuildTower(characterEQ.ValueRO.SelectedTower, entity, ref state);
							towerBuildCount.ValueRW.Count++;
						}
#if UNITY_EDITOR
						else
							Debug.Log("Not enough resources to build tower");
#endif
						break;
				}
			}
		}
	}

	private void BuildTower(Entity selectedTower, Entity highlighter, ref SystemState state)
	{
		Entity towerEntity = state.EntityManager.Instantiate(selectedTower);
		var towerTransform = state.EntityManager.GetComponentData<LocalTransform>(towerEntity);
		var highlighterTransform = state.EntityManager.GetComponentData<LocalToWorld>(highlighter);
		towerTransform.Position = highlighterTransform.Position;
		state.EntityManager.SetComponentData(towerEntity, towerTransform);
	}
}
