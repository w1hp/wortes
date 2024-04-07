using Unity.Entities;
using UnityEngine;

public partial class CharacterModeSystem : SystemBase
{
	Entity inputEntity;
	InputsData inputsData;
	Mode mode = Mode.Shoot;

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


		if (inputsData.switchMode)
		{
			if (mode == Mode.Build)
			{
				mode = Mode.Shoot;
				Debug.Log("Shoot Mode");

			}
			else
			{
				mode = Mode.Build;
				Debug.Log("Build Mode");
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

