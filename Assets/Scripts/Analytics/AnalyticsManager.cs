using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Threading.Tasks;

public class AnalyticsManager : MonoBehaviour
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

public class LevelEndedEvent : Unity.Services.Analytics.Event
{
	public LevelEndedEvent() : base("levelEnded")
	{
	}

	public int EnemyCount { set { SetParameter("enemyCount", value); } }
	public int Level_ID { set { SetParameter("level_ID", value); } }
	public bool LevelSuccess { set { SetParameter("levelSuccess", value); } }
	public int PlayerFragCount { set { SetParameter("playerFragCount", value); } }
	public int PlayerHealth { set { SetParameter("playerHealth", value); } }
	public float Time { set { SetParameter("time", value); } }
	public int TowerCount { set { SetParameter("towerCount", value); } }
	public int TowerFragCount { set { SetParameter("towerFragCount", value); } }
	public int UserLevel { set { SetParameter("userLevel", value); } }
}

public class LevelUpPlayerEvent : Unity.Services.Analytics.Event
{
	public LevelUpPlayerEvent() : base("levelUpPlayer")
	{
	}

	public string BuffName { set { SetParameter("buffName", value); } }
	public int Level_ID { set { SetParameter("level_ID", value); } }
	public int UserLevel { set { SetParameter("userLevel", value); } }
}