using Unity.Entities;
using Unity.Rendering;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Physics.Stateful;
using Unity.Assertions;


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


		foreach (var (triggerEventBuffer, materialChanger, baseColor, entity) in
			SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>, RefRO<MaterialChanger>, RefRW<URPMaterialPropertyBaseColor>>()
				.WithEntityAccess())
		{
			var character = SystemAPI.GetComponentRO<CharacterComponent>(materialChanger.ValueRO.Character);

			if (!character.ValueRO.IsBuildMode)
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.HighlighterPrefabEntity, false);
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.GunPrefabEntity, true);
				break;
			}

			SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.HighlighterPrefabEntity, true);
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
					//RED
					baseColor.ValueRW.Value = new float4(1, 0, 0, 0.5f);
				}
				else if (triggerEvent.State == StatefulEventState.Exit)
				{
					//GREEN
					baseColor.ValueRW.Value = new float4(0, 1, 0, 0.5f);
				}
			}
		}

	}
}