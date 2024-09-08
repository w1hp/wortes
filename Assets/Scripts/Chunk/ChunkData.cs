using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct ChunkData : IComponentData
{
    public float3 Position;
    public float ActivationDistance;
    public bool IsActive;
}
