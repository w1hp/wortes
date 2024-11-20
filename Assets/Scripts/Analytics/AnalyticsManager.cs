using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class AnalyticsManager : MonoBehaviour
{
	[SerializeField] private bool isTimeToCallEvent = false;
	async void Start()
	{
		await UnityServices.InitializeAsync();
		//List<string> consentIdentifiers = await Events.CheckForRequiredConsents();
		AnalyticsService.Instance.StartDataCollection();

	}
	public void OnTutorialStarted()
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>()
		{
			// { PARAMETER_KEY, PARAMETER_VALUE }
		};

		//Events.CustomData("tutorial_started", parameters);
	}

	void Update()
	{
		if (isTimeToCallEvent)
		{
			CallEvent();
			isTimeToCallEvent = false;
		}

	}
	public void CallEvent()
	{
		CustomEvent customEvent = new CustomEvent("gameplay");
		AnalyticsService.Instance.RecordEvent(customEvent);
	}
}
