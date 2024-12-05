//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Services.Analytics;

//[BurstCompile]
//public partial struct LowFrameRateAnalyticsSystem : ISystem
//{
//    public void OnCreate(ref SystemState state)
//    {
//        state.RequireForUpdate<LowFrameRateEventComponent>();
//    }

//    public void OnUpdate(ref SystemState state)
//    {
//        var ecb = new EntityCommandBuffer(Allocator.Temp);

//        foreach (var (eventData, entity) in SystemAPI.Query<RefRO<LowFrameRateEventComponent>>().WithEntityAccess())
//        {
//            AnalyticsService.Instance.RecordEvent(new LowFrameRateEvent
//            {
//                FrameRate = eventData.ValueRO.FrameRate,
//                LevelID = eventData.ValueRO.LevelID,
//                UserLevel = eventData.ValueRO.UserLevel,
//                Time = eventData.ValueRO.Time
//            });

//            ecb.RemoveComponent<LowFrameRateEventComponent>(entity);
//        }

//        ecb.Playback(state.EntityManager);
//        ecb.Dispose();
//    }
//}
