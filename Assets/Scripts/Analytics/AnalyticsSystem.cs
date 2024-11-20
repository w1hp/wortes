using Unity.Burst;
using Unity.Collections;
using System.Collections.Generic;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct AnalyticsSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<AnalyticsEvent>();
	}

	public void OnUpdate(ref SystemState state)
	{
		//var eventQuery = SystemAPI.Query<AnalyticsEvent>();
		foreach (var (analyticsEvent, entity) in SystemAPI.Query<AnalyticsEvent>().WithEntityAccess())
		{
			// Send event to Unity Analytics
			//UnityEngine.Analytics.Analytics.CustomEvent(
			//	analyticsEvent.EventName.ToString(),
			//	new Dictionary<string, object>
			//	{
			//		{ analyticsEvent.ParameterKey.ToString(), analyticsEvent.ParameterValue }
			//	});

			// Optionally remove the component to avoid re-sending the same event
			state.EntityManager.RemoveComponent<AnalyticsEvent>(entity);
		}
	}
}

public struct AnalyticsEvent : IComponentData
{
	public FixedString128Bytes EventName;
	public FixedString128Bytes ParameterKey;
	public float ParameterValue;
}