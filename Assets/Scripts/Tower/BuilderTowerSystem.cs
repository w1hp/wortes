using Unity.Burst;
using Unity.Entities;
using UnityEditor;
using UnityEngine.TextCore.Text;

partial struct BuilderTowerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
	}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
