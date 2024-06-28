using Unity.Burst;
using Unity.Entities;
using UnityEditor;
using UnityEngine.TextCore.Text;

partial struct BuilderTowerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BuilderTower>();
	}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (builderTower, materialChanger) in SystemAPI.Query<BuilderTower, MaterialChanger>())
        {
			if (true)
            {


			}
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
