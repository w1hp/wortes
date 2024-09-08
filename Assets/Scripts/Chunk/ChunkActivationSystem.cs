using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class ChunkActivationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Get player position from the PlayerData component
        float3 playerPosition = float3.zero;

        Entities.ForEach((ref PlayerData playerData) =>
        {
            playerPosition = playerData.Position;
        }).WithoutBurst().Run();

        // Iterate over all chunks and check distance to the player
        Entities.ForEach((ref ChunkData chunkData, in LocalToWorld chunkTransform) =>
        {
            float distance = math.distance(playerPosition, chunkTransform.Position);

            if (distance <= chunkData.ActivationDistance)
            {
                // Activate chunk
                chunkData.IsActive = true;
            }
            else
            {
                // Deactivate chunk
                chunkData.IsActive = false;
            }
        }).ScheduleParallel();
    }
}
