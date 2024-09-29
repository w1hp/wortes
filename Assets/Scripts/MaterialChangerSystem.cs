using Unity.Entities;
using Unity.Rendering;
using Unity.Burst;
using Unity.Physics.Stateful;
using Unity.Assertions;
using Unity.Transforms;


[BurstCompile]
public partial struct MaterialChangerSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<MaterialChanger>();
		state.RequireForUpdate<CharacterEquipment>();
		state.RequireForUpdate<CharacterComponent>();
	}
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var nonTriggerQuery = SystemAPI.QueryBuilder().WithNone<StatefulTriggerEvent>().Build();
		Assert.IsFalse(nonTriggerQuery.HasFilter(),
			"The use of EntityQueryMask in this system will not respect the query's active filter settings.");
		var nonTriggerMask = nonTriggerQuery.GetEntityQueryMask();

		//var materialMeshInfoLookup = SystemAPI.GetComponentLookup<MaterialMeshInfo>();

		foreach (var (triggerEventBuffer, materialChanger, entity) in
			SystemAPI.Query<
				DynamicBuffer<StatefulTriggerEvent>,
				RefRW<MaterialChanger>>()
				.WithEntityAccess())
		{
			RefRO<CharacterEquipment> characterEQ = SystemAPI.GetComponentRO<CharacterEquipment>(materialChanger.ValueRO.Character);
			RefRO<CharacterComponent> character = SystemAPI.GetComponentRO<CharacterComponent>(materialChanger.ValueRO.Character);


			var highlighterChildren = SystemAPI.GetBuffer<Child>(entity);
			var graphicEntity = highlighterChildren[0].Value;


			if (!character.ValueRO.IsBuildMode)
			{
				// show choosen gun
				{
					SystemAPI.SetComponentEnabled<MaterialMeshInfo>(graphicEntity, false);

					switch (character.ValueRO.CurrentSlot)
					{
						case 0:
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot1, true);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot2, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot3, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot4, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot5, false);
							break;
						case 1:
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot1, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot2, true);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot3, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot4, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot5, false);
							break;
						case 2:
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot1, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot2, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot3, true);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot4, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot5, false);
							break;
						case 3:
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot1, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot2, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot3, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot4, true);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot5, false);
							break;
						case 4:
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot1, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot2, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot3, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot4, false);
							SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot5, true);
							break;
					}
					//SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.GunPrefabEntity, true);
					break;
				}
			}
			// show higlighter
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(graphicEntity, true);

				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot1, false);
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot2, false);
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot3, false);
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot4, false);
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(characterEQ.ValueRO.WeaponSlot5, false);
				//SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.GunPrefabEntity, false);
			}
		}
	}
}