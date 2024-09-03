using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class LevelUpSystem : SystemBase
{
	public event Action LevelUp;

	protected override void OnCreate()
	{
		RequireForUpdate(SystemAPI.QueryBuilder().WithAll<CharacterComponent, Inventory>().Build());
		//RequireForUpdate<IsNotPause>();
	}

	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);
		foreach (var(characterComponent, inventory) in 
			SystemAPI.Query<RefRO<CharacterComponent>, RefRW<Inventory>>())
		{
			if (CanLevelUp(inventory.ValueRO.Gold, inventory.ValueRO.Level))
			{
				Debug.Log("Level Up!");
				//TODO: zmienic to kiedys na cos bardziej sensownego
				UnityEngine.Time.timeScale = 0;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.Confined;

				//var entity = SystemAPI.GetSingletonEntity<IsNotPause>();
				//ECB.AddComponent<Disabled>(entity);

				//Debug.Log(RandomNumberUnityMathematics(3));


				inventory.ValueRW.Level++;
				LevelUp?.Invoke();


			}
		}
	}

	bool CanLevelUp(float gold, float level)
	{
		return gold >= 10 * math.pow(level,2);
	}

	(int, int, int) RandomNumberUnityMathematics(int numberOfPowerUps)
	{
		uint seed = 69;
		Unity.Mathematics.Random rng = new Unity.Mathematics.Random(seed);
		int randomInt1 = rng.NextInt(numberOfPowerUps);
		int randomInt2 = rng.NextInt(numberOfPowerUps);
		int randomInt3 = rng.NextInt(numberOfPowerUps);

		while (randomInt1 == randomInt2 || randomInt1 == randomInt3 || randomInt2 == randomInt3)
		{
			randomInt1 = rng.NextInt(numberOfPowerUps);
			randomInt2 = rng.NextInt(numberOfPowerUps);
			randomInt3 = rng.NextInt(numberOfPowerUps);
		}

		return (randomInt1, randomInt2, randomInt3);
	}
}