using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial class LevelUpSystem : SystemBase
{
	public event Action<int> LevelUp;

	protected override void OnCreate()
	{
		RequireForUpdate(SystemAPI.QueryBuilder().WithAll<CharacterComponent, CharacterResources>().Build());
		//RequireForUpdate<IsNotPause>();
	}

	protected override void OnUpdate()
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
		var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);
		foreach (var (characterComponent, characterResources) in
			SystemAPI.Query<RefRO<CharacterComponent>, RefRW<CharacterResources>>())
		{
			if (CanLevelUp(characterResources.ValueRO.Gold, characterResources.ValueRO.Level))
			{

#if UNITY_EDITOR
				Debug.Log("Level Up!");
#endif

				PauseMenuController.Singleton.SetPause(true);

				//Cursor.visible = true;
				//Cursor.lockState = CursorLockMode.Confined;

				LevelUp?.Invoke((int)characterResources.ValueRW.Level);
				characterResources.ValueRW.Level++;
			}
		}
	}

	public void ReturnToGame(PowerUpType type, int value)
	{
#if UNITY_EDITOR
		Debug.Log("Return to game");
#endif
		PauseMenuController.Singleton.SetPause(false);

		foreach (var (characterComponent, health, characterStats, purchasedUpgrades, entity) in
						SystemAPI.Query<RefRW<CharacterComponent>, RefRW<Health>, RefRW<BaseStats>, RefRO<PurchasedUpgrades>>().WithEntityAccess())
		{
			var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
			var ECB = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);

			// Add const power up per level
			var moveSpeedStatModification = new MoveSpeedStatModification
			{
				ModificationType = StatModificationTypes.Percentage,
				ModificationValue = purchasedUpgrades.ValueRO.MoveSpeed * 0.05f
			};

			//if (purchasedUpgrades.ValueRO.AttackSpeed > 0)
			var attackSpeedStatModification = new AttackSpeedStatModification
			{
				ModificationType = StatModificationTypes.Percentage,
				ModificationValue = purchasedUpgrades.ValueRO.AttackSpeed * 0.05f
			};

			var attackDamageStatModification = new AttackDamageStatModification
			{
				ModificationType = StatModificationTypes.Percentage,
				ModificationValue = purchasedUpgrades.ValueRO.AttackDamage * 0.05f
			};

			var healthStatModification = new HealthStatModification
			{
				ModificationType = StatModificationTypes.Percentage,
				ModificationValue = purchasedUpgrades.ValueRO.MaxHealth * 0.05f
			};

			// Add power up
			switch (type)
			{
				case PowerUpType.Health:
					healthStatModification.ModificationValue += value;

					//ECB.AddComponent(entity, new StatModification
					//{
					//	StatToModify = StatTypes.MaxHealth,
					//	ModificationType = StatModificationTypes.Percentage,
					//	ModificationValue = value
					//});
					//health.ValueRW.CurrentHealth += value;
					break;
				case PowerUpType.MoveSpeed:
					moveSpeedStatModification.ModificationValue += value;
					//ECB.AddComponent(entity, new StatModification
					//{
					//	StatToModify = StatTypes.MoveSpeed,
					//	ModificationType = StatModificationTypes.Percentage,
					//	ModificationValue = value
					//});
					//characterComponent.ValueRW.GroundRunMaxSpeed += value;
					break;
				case PowerUpType.Damage:
					attackDamageStatModification.ModificationValue += value;
					//ECB.AddComponent(entity, new StatModification
					//{
					//	StatToModify = StatTypes.AttackDamage,
					//	ModificationType = StatModificationTypes.Percentage,
					//	ModificationValue = value
					//});
					//characterStats.ValueRW.AttackDamage += value;
					break;
				case PowerUpType.AttackSpeed:
					attackSpeedStatModification.ModificationValue += value;
					break;
			}
			if (healthStatModification.ModificationValue > 0)
				ECB.AddComponent(entity, healthStatModification);
			if (moveSpeedStatModification.ModificationValue > 0)
				ECB.AddComponent(entity, moveSpeedStatModification);
			if (attackDamageStatModification.ModificationValue > 0)
				ECB.AddComponent(entity, attackDamageStatModification);
			if (attackSpeedStatModification.ModificationValue > 0)
				ECB.AddComponent(entity, attackSpeedStatModification);



		}
	}


	bool CanLevelUp(float gold, float level)
	{
		return gold >= 10 * math.pow(level, 2);
	}

	//(int, int, int) RandomNumberUnityMathematics(int numberOfPowerUps)
	//{
	//	uint seed = 69;
	//	Unity.Mathematics.Random rng = new Unity.Mathematics.Random(seed);
	//	int randomInt1 = rng.NextInt(numberOfPowerUps);
	//	int randomInt2 = rng.NextInt(numberOfPowerUps);
	//	int randomInt3 = rng.NextInt(numberOfPowerUps);

	//	while (randomInt1 == randomInt2 || randomInt1 == randomInt3 || randomInt2 == randomInt3)
	//	{
	//		randomInt1 = rng.NextInt(numberOfPowerUps);
	//		randomInt2 = rng.NextInt(numberOfPowerUps);
	//		randomInt3 = rng.NextInt(numberOfPowerUps);
	//	}

	//	return (randomInt1, randomInt2, randomInt3);
	//}
}