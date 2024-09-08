using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

partial struct UISystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SceneReference>();

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
			//var mainScene = scenes[0];
			selectedScene.LoadingAction = action;
			//mainScene.LoadingAction = action;
			state.EntityManager.SetComponentData(entities[sceneIndex], selectedScene);
			//state.EntityManager.SetComponentData(entities[0], mainScene);
		}
	}
}
