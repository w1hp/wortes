using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Services.Analytics;

namespace GameAnalytics
{
    // Komponent przechowujący dane o pokonanym przeciwniku
    public struct EnemyDefeatedEventComponent : IComponentData
    {
        public int EnemyID;      // ID pokonanego przeciwnika
        public int KillerID;     // ID zabójcy
        public int LevelID;      // ID poziomu, na którym przeciwnik został pokonany
    }

    // Klasa definiująca zdarzenie analityczne dla pokonanych przeciwników
    public class EnemyDefeatedEvent : Event
    {
        public EnemyDefeatedEvent(int enemyID, int killerID, int levelID) : base("enemyDefeated")
        {
            SetParameter("enemyID", enemyID);   // ID przeciwnika
            SetParameter("killerID", killerID); // ID zabójcy
            SetParameter("level_ID", levelID);  // ID poziomu
        }
    }

    // System do generowania zdarzeń EnemyDefeatedEventComponent
    [BurstCompile]
    public partial struct EnemyDefeatedDetectionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // Inicjalizacja danych, jeśli konieczne
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            // Przykład logiki wykrywania pokonanych przeciwników
            foreach (var (enemy, entity) in SystemAPI.Query<RefRO<EnemyComponent>>().WithEntityAccess())
            {
                if (enemy.ValueRO.Health <= 0) // Jeśli przeciwnik został pokonany
                {
                    ecb.AddComponent(entity, new EnemyDefeatedEventComponent
                    {
                        EnemyID = enemy.ValueRO.ID,
                        KillerID = enemy.ValueRO.KillerID,
                        LevelID = GetCurrentLevelID() // Funkcja do pobrania aktualnego poziomu
                    });
                }
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

    // System do analityki pokonanych przeciwników
    [BurstCompile]
    public partial struct EnemyDefeatedAnalyticsSystem : ISystem
    {
        private NativeHashMap<int, int> levelKillCounts;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyDefeatedEventComponent>();
            levelKillCounts = new NativeHashMap<int, int>(16, Allocator.Persistent);
        }

        public void OnDestroy(ref SystemState state)
        {
            if (levelKillCounts.IsCreated)
            {
                levelKillCounts.Dispose();
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (eventData, entity) in SystemAPI.Query<RefRO<EnemyDefeatedEventComponent>>().WithEntityAccess())
            {
                int levelID = eventData.ValueRO.LevelID;

                // Aktualizuj licznik zabójstw dla poziomu
                if (levelKillCounts.ContainsKey(levelID))
                {
                    levelKillCounts[levelID]++;
                }
                else
                {
                    levelKillCounts[levelID] = 1;
                }

                // Rejestracja zdarzenia w analityce
                AnalyticsService.Instance.RecordEvent(new EnemyDefeatedEvent(
                    eventData.ValueRO.EnemyID,
                    eventData.ValueRO.KillerID,
                    eventData.ValueRO.LevelID
                ));

                // Usuń komponent po przetworzeniu
                ecb.RemoveComponent<EnemyDefeatedEventComponent>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        public int GetKillCountForLevel(int levelID)
        {
            if (levelKillCounts.ContainsKey(levelID))
            {
                return levelKillCounts[levelID];
            }

            return 0; // Jeśli brak danych, zwróć 0
        }
    }

    // Komponent przeciwnika (przykład)
    public struct EnemyComponent : IComponentData
    {
        public int ID;         // ID przeciwnika
        public int KillerID;   // ID zabójcy
        public float Health;   // Aktualne zdrowie przeciwnika
    }
}
