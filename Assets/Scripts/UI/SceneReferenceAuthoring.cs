using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;


public class SceneReferenceAuthoring : MonoBehaviour
{
#if UNITY_EDITOR

	public List<UnityEditor.SceneAsset> sceneAssets;

	class Baker : Baker<SceneReferenceAuthoring>
	{
		public override void Bake(SceneReferenceAuthoring authoring)
		{
			foreach (var sceneAsset in authoring.sceneAssets)
			{
				DependsOn(sceneAsset);
				if (sceneAsset != null)
				{
					var entity = CreateAdditionalEntity(TransformUsageFlags.Dynamic, false, sceneAsset.name);
					AddComponent(entity, new SceneReference
					{
						SceneName = new FixedString128Bytes(sceneAsset.name),
						SceneAsset = new EntitySceneReference(sceneAsset),
						StreamingState = default,
						EntityScene = default
					});
				}
			}
		}

	}
#endif
}


public enum LoadingAction
{
	None = 0, // No action required
	LoadAll = 1, // Loads the scene and section entities and the entities in each section
	LoadMeta = 2, // Loads the scene and section entities but not the entities in each section
	UnloadEntities = 4, // Unloads the entities in each section, but it keeps loaded the scene and section entities
	UnloadAll = 8 // Unloads the scene and section entities as well as the content of each section
}

public struct SceneReference : IComponentData
{
	public FixedString128Bytes SceneName; // Name of the scene (To display it in the UI)
	public EntitySceneReference SceneAsset; // Reference to the scene (Used for streaming in the scene)
	public SceneSystem.SceneStreamingState StreamingState; // Current state of the loading scene
	public LoadingAction LoadingAction; // Action requested from the UI

	// Entity representing the scene once the scene is loaded (even while the sections are not loaded).
	public Entity EntityScene;
}
