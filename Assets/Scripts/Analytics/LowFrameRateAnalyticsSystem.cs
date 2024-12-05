using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Services.Analytics;

namespace GameAnalytics
{
    // Komponent przechowujący dane o spadku FPS
    public struct LowFrameRateEventComponent : IComponentData
    {
        public float FrameRate; // Aktualny FPS w momencie zdarzenia
        public int LevelID;     // ID poziomu
        public float TimeBelowThreshold; // Czas spędzony poniżej 90 klatek
    }

    // Klasa definiująca zdarzenie analityczne dla spadków FPS
    public class LowFrameRateEvent : Event
    {
        public LowFrameRateEvent() : base(name: "lowFrameRate")
        {
        }

        public float FrameRate { set { SetParameter(name: "frameRate", value); } }
        public int LevelID { set { SetParameter(name: "level_ID", value); } }
        public float TimeBelowThreshold { set { SetParameter(name: "timeBelowThreshold", value); } }
    }

    // System monitorujący FPS i generujący zdarzenia LowFrameRateEventComponent
    [BurstCompile]
    public partial struct LowFrameRateDetectionSystem : ISystem
    {
        private float timeBelowThreshold;

        public void OnCreate(ref SystemState state)
        {
            timeBelowThreshold = 0f;
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            float currentFrameRate = 1.0f / UnityEngine.Time.deltaTime;

            // Pobierz czas gry z SystemAPI
            float deltaTime = SystemAPI.Time.DeltaTime;

            if (currentFrameRate < 90)
            {
                // Dodaj czas spędzony poniżej 90 klatek
                timeBelowThreshold += deltaTime;

                Entity entity = state.EntityManager.CreateEntity();
                ecb.AddComponent(entity, new LowFrameRateEventComponent
                {
                    FrameRate = currentFrameRate,
                    LevelID = GetCurrentLevelID(), // Funkcja do pobrania aktualnego ID poziomu
                    TimeBelowThreshold = timeBelowThreshold // Zebrany czas
                });
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        private int GetCurrentLevelID()
        {
            // Placeholder - zaimplementuj własną logikę do pobrania aktualnego poziomu
            return 1;
        }
    }

    // System przesyłający dane o spadkach FPS do analityki i obliczający średni FPS
    [BurstCompile]
    public partial struct LowFrameRateAnalyticsSystem : ISystem
    {
        private NativeHashMap<int, (float totalFrameRate, int sampleCount)> levelFrameData;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<LowFrameRateEventComponent>();
            levelFrameData = new NativeHashMap<int, (float totalFrameRate, int sampleCount)>(16, Allocator.Persistent);
        }

        public void OnDestroy(ref SystemState state)
        {
            if (levelFrameData.IsCreated)
            {
                levelFrameData.Dispose();
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (eventData, entity) in SystemAPI.Query<RefRO<LowFrameRateEventComponent>>().WithEntityAccess())
            {
                // Aktualizuj dane dla średniego FPS na poziomie
                int levelID = eventData.ValueRO.LevelID;
                float frameRate = eventData.ValueRO.FrameRate;

                if (levelFrameData.ContainsKey(levelID))
                {
                    var data = levelFrameData[levelID];
                    data.totalFrameRate += frameRate;
                    data.sampleCount++;
                    levelFrameData[levelID] = data;
                }
                else
                {
                    levelFrameData[levelID] = (frameRate, 1);
                }

                // Rejestracja zdarzenia w analityce
                AnalyticsService.Instance.RecordEvent(new LowFrameRateEvent
                {
                    FrameRate = eventData.ValueRO.FrameRate,
                    LevelID = eventData.ValueRO.LevelID,
                    TimeBelowThreshold = eventData.ValueRO.TimeBelowThreshold
                });

                // Usuń komponent po przetworzeniu
                ecb.RemoveComponent<LowFrameRateEventComponent>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        public float GetAverageFrameRate(int levelID)
        {
            if (levelFrameData.ContainsKey(levelID))
            {
                var data = levelFrameData[levelID];
                return data.totalFrameRate / data.sampleCount;
            }

            return 0f; // Jeśli brak danych, zwróć 0
        }
    }
}
