using Unity.Entities;
using Unity.Rendering;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public partial struct MaterialChangerSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var character in SystemAPI.Query<RefRW<CharacterComponent>>().WithNone<Prefab>())
		{
			if (character.ValueRO.IsBuildMode)
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.HighlighterPrefabEntity, true);
				
				var baseColor = SystemAPI.GetComponent<URPMaterialPropertyBaseColor>(character.ValueRO.HighlighterPrefabEntity);
				baseColor.Value = new float4(0, 1, 0, 0.5f);
				SystemAPI.SetComponent<URPMaterialPropertyBaseColor>(character.ValueRO.HighlighterPrefabEntity, baseColor);
			}
			else
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.HighlighterPrefabEntity, false);
			}
		}
	}
}