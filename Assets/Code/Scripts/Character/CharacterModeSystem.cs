using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public partial class CharacterModeSystem : SystemBase
{
	Entity inputEntity;
	InputsData inputsData;
	CharacterModel characterModel;
	Mode mode = Mode.Shoot;
	Entity highlighterEntity;
	Entity gunEntity;


	protected override void OnCreate()
	{
		if (!SystemAPI.TryGetSingleton<CharacterMode>(out CharacterMode mode))
		{
			EntityManager.CreateEntity(typeof(CharacterMode));
		}
	}

	protected override void OnUpdate()
	{
		inputEntity = SystemAPI.GetSingletonEntity<InputsData>();
		inputsData = SystemAPI.GetComponent<InputsData>(inputEntity);

		characterModel = SystemAPI.GetSingleton<CharacterModel>();
		gunEntity = characterModel.gun;
		highlighterEntity = characterModel.highlighter;

		if (inputsData.switchMode)
		{
			if (mode == Mode.Build)
			{
				Debug.Log("Shoot Mode");
				mode = Mode.Shoot;
				EntityManager.AddComponent(highlighterEntity, typeof(Disabled));
				EntityManager.RemoveComponent<Disabled>(gunEntity);
			}
			else
			{
				Debug.Log("Build Mode");
				mode = Mode.Build;
				EntityManager.AddComponent(gunEntity, typeof(Disabled));
				EntityManager.RemoveComponent<Disabled>(highlighterEntity);
			}
		}
		SystemAPI.SetSingleton(new CharacterMode
		{
			mode = mode
		});


		if (mode == Mode.Shoot)
		{

			// TODO : Implement shoot mode
		}
		else
		{
			// TODO : Implement build mode
			bool canBuild = false;
			EntityManager.AddComponent<URPMaterialPropertyBaseColor>(highlighterEntity);

			if (canBuild)
			{
				// set highlighter to green
				EntityManager.SetComponentData(highlighterEntity, new URPMaterialPropertyBaseColor
				{
					Value = new float4(0, 1, 0, 0.5f)
				});
			}
			else
			{
				// set highlighter to red
				EntityManager.SetComponentData(highlighterEntity, new URPMaterialPropertyBaseColor
				{
					Value = new float4(1, 0, 0, 0.5f)
				});
			}
		}
	}
}

public struct CharacterMode : IComponentData
{
	public Mode mode;
}

public enum Mode
{
	Build,
	Shoot
}

