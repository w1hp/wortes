using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

partial struct UISystem : ISystem
{
	
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SceneReference>();
		//isAnySceneUnloading = false;
	}

	public void OnUpdate(ref SystemState state)
	{

		if (StateUI.Singleton == null)
		{
			return;
		}
		var ui = StateUI.Singleton;

		var sceneQuery = SystemAPI.QueryBuilder().WithAll<SceneReference>().Build();
		var scenes = sceneQuery.ToComponentDataArray<SceneReference>(Allocator.Temp);
		var entities = sceneQuery.ToEntityArray(Allocator.Temp);

		if (ui.GetAction(out var sceneIndex, out var action))
		{
			var selectedScene = scenes[sceneIndex];
			selectedScene.LoadingAction = action;
			state.EntityManager.SetComponentData(entities[sceneIndex], selectedScene);
		}
		//int processingSceneCount = 0;
		bool isAnySceneUnloading = false;
		for (int index = 0; index < scenes.Length; ++index)
		{
			var scene = scenes[index];
			if (scene.StreamingState == Unity.Scenes.SceneSystem.SceneStreamingState.Unloading ||
				scene.StreamingState == Unity.Scenes.SceneSystem.SceneStreamingState.Loading)
			{

//#if UNITY_EDITOR
//				UnityEngine.Debug.Log("(Un)loading");
//#endif
				isAnySceneUnloading = true;
				break;
			}
			else
			{
				isAnySceneUnloading = false;
			}
		}

		ui.loadingPanel.SetActive(isAnySceneUnloading);
	}
}