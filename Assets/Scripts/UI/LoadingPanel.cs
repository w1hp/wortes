using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Scenes;


public class LoadingPanel : MonoBehaviour
{
	private EntityManager _entityManager;
	private bool isLoading = false;

	private void OnEnable()
	{
		isLoading = true;
    }
	private void OnDisable()
	{
		isLoading = false;
	}
	private void Update()
	{
		if (isLoading)
		{
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			var sceneQuery = _entityManager.CreateEntityQuery(typeof(SceneReference));
			var scenes = sceneQuery.ToComponentDataArray<SceneReference>(Allocator.Temp);

			foreach (var scene in scenes)
			{
				if (scene.StreamingState == SceneSystem.SceneStreamingState.LoadedSuccessfully)
				{
#if UNITY_EDITOR
					Debug.Log("LoadedSuccessfully");
#endif
					gameObject.SetActive(false);
				}
			}
		}
	}
}

