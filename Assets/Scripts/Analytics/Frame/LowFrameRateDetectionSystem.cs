//using Unity.Burst;
//using Unity.Entities;
//using Unity.Collections;

//[BurstCompile]
//public partial struct LowFrameRateDetectionSystem : ISystem
//{
//    private float _currentTime;

//    public void OnCreate(ref SystemState state)
//    {
//        _currentTime = 0f;
//    }

//    public void OnUpdate(ref SystemState state)
//    {
//        var ecb = new EntityCommandBuffer(Allocator.Temp);
//        float currentFrameRate = 1.0f / UnityEngine.Time.deltaTime;

//        // Pobierz czas gry z SystemAPI i dokonaj jawnej konwersji na float
//        float elapsedTime = (float)SystemAPI.Time.ElapsedTime;

//        // Sprawdź, czy FPS spadł poniżej progu
//        if (currentFrameRate < 90)
//        {
//            Entity entity = state.EntityManager.CreateEntity();
//            ecb.AddComponent(entity, new LowFrameRateEventComponent
//            {
//                FrameRate = currentFrameRate,
//                LevelID = GetCurrentLevelID(), // Funkcja do pobrania ID poziomu
//                UserLevel = GetCurrentUserLevel(), // Funkcja do pobrania poziomu gracza
//                Time = elapsedTime // Czas gry (już jako float)
//            });
//        }

//        _currentTime += SystemAPI.Time.DeltaTime; // DeltaTime jest już typu float
//        ecb.Playback(state.EntityManager);
//        ecb.Dispose();
//    }

//    private int GetCurrentLevelID()
//    {
//        // Tutaj zaimplementuj logikę zwracającą aktualny LevelID
//        return 1; // Placeholder
//    }

//    private int GetCurrentUserLevel()
//    {
//        // Tutaj zaimplementuj logikę zwracającą aktualny poziom gracza
//        return 1; // Placeholder
//    }
//}
