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
		//var baseColorLookup = SystemAPI.GetComponentLookup<URPMaterialPropertyBaseColor>();
		foreach (var character in SystemAPI.Query<RefRW<CharacterComponent>>().WithNone<Prefab>())
		{
			if (character.ValueRO.IsBuildMode)
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.HighlighterPrefabEntity, true);
				//var baseColor = SystemAPI.GetComponent<URPMaterialPropertyBaseColor>(character.ValueRW.HighlighterPrefabEntity);
				//baseColor.ValueRW.Value = new float4(0, 1, 0, 0.5f);
				var baseColor = SystemAPI.GetComponent<URPMaterialPropertyBaseColor>(character.ValueRW.HighlighterPrefabEntity);
				baseColor.Value = new float4(0, 1, 0, 0.5f);
				SystemAPI.SetComponent<URPMaterialPropertyBaseColor>(character.ValueRW.HighlighterPrefabEntity, baseColor);
				//baseColorLookup[character.ValueRO.HighlighterPrefabEntity].Value = new float4(0, 1, 0, 0.5f);

			}
			else
			{
				SystemAPI.SetComponentEnabled<MaterialMeshInfo>(character.ValueRO.HighlighterPrefabEntity, false);
			}
		}



		//foreach (var(matChanger, baseColor) in SystemAPI.Query<MaterialChanger, RefRW<URPMaterialPropertyBaseColor>>())
		//{
		//	if (matChanger.isRed)
		//	{
		//		baseColor.ValueRW.Value = new float4(1, 0, 0, 0.5f);
		//	}
		//	else
		//	{
		//		baseColor.ValueRW.Value = new float4(0, 1, 0, 0.5f);
		//	}
		//}
	}
}