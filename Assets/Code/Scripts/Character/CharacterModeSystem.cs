using Unity.Entities;
using UnityEngine;

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

