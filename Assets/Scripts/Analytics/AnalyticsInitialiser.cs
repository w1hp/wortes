using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Threading.Tasks;
using UnityEngine;

public class AnalyticsInitialiser : MonoBehaviour
{
    private async void Start()
    {
        await InitializeUnityServices();
    }
    private async Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            if (AnalyticsService.Instance != null)
            {
                AnalyticsService.Instance.StartDataCollection();
                Debug.Log("Unity Analytics initialized and data collection started.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
        }
    }

    public void LogLowFrameRateEvent(float frameRate, int levelID, float timeBelowThreshold)
    {
        var lowFrameRateEvent = new GameAnalytics.LowFrameRateEvent
        {
            FrameRate = frameRate,
            LevelID = levelID,
            TimeBelowThreshold = timeBelowThreshold
        };

        AnalyticsService.Instance.RecordEvent(lowFrameRateEvent);
        Debug.Log($"Logged LowFrameRateEvent: FrameRate={frameRate}, LevelID={levelID}, TimeBelowThreshold={timeBelowThreshold}");
    }

    public void LogEnemyDefeatedEvent(int enemyID, int killerID, int levelID)
    {
        var enemyDefeatedEvent = new GameAnalytics.EnemyDefeatedEvent(enemyID, killerID, levelID);
        AnalyticsService.Instance.RecordEvent(enemyDefeatedEvent);
        Debug.Log($"Logged EnemyDefeatedEvent: EnemyID={enemyID}, KillerID={killerID}, LevelID={levelID}");
    }

    public void LogUpgradeChoiceEvent(int levelID, int upgradeID)
    {
        var upgradeChoiceEvent = new GameAnalytics.UpgradeChoiceEvent(levelID, upgradeID.ToString());
        AnalyticsService.Instance.RecordEvent(upgradeChoiceEvent);
        Debug.Log($"Logged UpgradeChoiceEvent: LevelID={levelID}, UpgradeID={upgradeID}");
    }

}
