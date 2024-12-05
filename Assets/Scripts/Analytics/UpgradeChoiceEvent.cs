using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Services.Analytics;

namespace GameAnalytics
{
    // Komponent przechowujący dane o wyborze ulepszenia
    public struct UpgradeChoiceEventComponent : IComponentData
    {
        public int LevelID;                 // ID poziomu
        public FixedString64Bytes UpgradeID; // ECS-compatible FixedString
    }

    // Klasa definiująca zdarzenie analityczne dla wyboru ulepszenia
    public class UpgradeChoiceEvent : Unity.Services.Analytics.Event
    {
        public UpgradeChoiceEvent(int levelID, string upgradeID) : base("upgradeChoiceV2")
        {
            SetParameter("lvl_ID", levelID);
            SetParameter("UpgradeID", upgradeID); // "Health", "Damage", "Speed"
        }
    }

    // System do wykrywania wyborów ulepszeń
    [BurstCompile]
    public partial struct UpgradeChoiceDetectionSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (player, entity) in SystemAPI.Query<RefRW<PlayerComponent>>().WithEntityAccess())
            {
                if (!string.IsNullOrEmpty(player.ValueRO.UpgradeChosen.ToString())) // Convert FixedString64Bytes to string
                {
                    ecb.AddComponent(entity, new UpgradeChoiceEventComponent
                    {
                        LevelID = player.ValueRO.CurrentLevel,
                        UpgradeID = new FixedString64Bytes(player.ValueRO.UpgradeChosen) // FixedString for ECS compatibility
                    });

                    player.ValueRW.UpgradeChosen = default; // Reset wyboru (FixedString)
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    // System do analizy wyborów ulepszeń
    [BurstCompile]
    public partial struct UpgradeChoiceAnalyticsSystem : ISystem
    {
        private NativeHashMap<int, int> upgradeCounts;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<UpgradeChoiceEventComponent>();
            upgradeCounts = new NativeHashMap<int, int>(3, Allocator.Persistent);
            upgradeCounts[0] = 0; // Health
            upgradeCounts[1] = 0; // Damage
            upgradeCounts[2] = 0; // Speed
        }

        public void OnDestroy(ref SystemState state)
        {
            if (upgradeCounts.IsCreated)
            {
                upgradeCounts.Dispose();
            }
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (eventData, entity) in SystemAPI.Query<RefRO<UpgradeChoiceEventComponent>>().WithEntityAccess())
            {
                string upgradeIDStr = eventData.ValueRO.UpgradeID.ToString(); // Convert FixedString64Bytes to string
                var upgradeID = GetUpgradeID(upgradeIDStr);

                if (upgradeID >= 0 && upgradeCounts.ContainsKey(upgradeID))
                {
                    upgradeCounts[upgradeID]++;
                    AnalyticsService.Instance.RecordEvent(new UpgradeChoiceEvent(
                        eventData.ValueRO.LevelID,
                        upgradeIDStr
                    ));
                }

                ecb.RemoveComponent<UpgradeChoiceEventComponent>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        private int GetUpgradeID(string upgrade)
        {
            return upgrade switch
            {
                "Health" => 0,
                "Damage" => 1,
                "Speed" => 2,
                _ => -1 // Unknown upgrade
            };
        }

        public int GetUpgradeCount(int upgradeID)
        {
            if (upgradeCounts.ContainsKey(upgradeID))
            {
                return upgradeCounts[upgradeID];
            }

            return 0;
        }
    }

    // Komponent gracza
    public struct PlayerComponent : IComponentData
    {
        public int CurrentLevel;              // Aktualny poziom gracza
        public FixedString64Bytes UpgradeChosen; // Wybrane ulepszenie: "Health", "Damage", "Speed"
    }
}
