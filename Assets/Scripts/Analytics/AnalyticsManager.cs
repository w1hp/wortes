using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class AnalyticsManager : MonoBehaviour
{
	async void Start()
	{
		await UnityServices.InitializeAsync();
		AnalyticsService.Instance.StartDataCollection();
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