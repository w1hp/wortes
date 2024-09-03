using UnityEngine;
using UnityEngine.Localization;
using Unity.Entities;
using TMPro;
using System.Collections.Generic;

public class LevelUpController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject container;
	[SerializeField] private TextMeshProUGUI levelUpText;

	[SerializeField] private TextMeshProUGUI namePowerUpText1;
	[SerializeField] private TextMeshProUGUI descriptionPowerUpText1;

	[SerializeField] private TextMeshProUGUI namePowerUpText2;
	[SerializeField] private TextMeshProUGUI descriptionPowerUpText2;

	[SerializeField] private TextMeshProUGUI namePowerUpText3;
	[SerializeField] private TextMeshProUGUI descriptionPowerUpText3;

	[SerializeField] private List<PowerUpSO> poolOfPowerUps;

	[Header("Localization")]
	[SerializeField] private LocalizedString namePowerUpLocalization;
	[SerializeField] private LocalizedString descriptionPowerUpLocalization;

	private LevelUpSystem levelUpSystem;

	private void Awake()
	{
		levelUpSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<LevelUpSystem>();
		container.SetActive(false);
	}

	private void OnEnable()
	{
		levelUpSystem.LevelUp += OnLevelUpEvent;
	}

	private void OnDisable()
	{
		levelUpSystem.LevelUp -= OnLevelUpEvent;
	}

	private void OnLevelUpEvent()
	{
		container.SetActive(true);

		var (randomInt1, randomInt2, randomInt3) = Get3RandomPowerUps(poolOfPowerUps.Count);

		namePowerUpText1.text = poolOfPowerUps[randomInt1].powerUpName.GetLocalizedString();
		descriptionPowerUpText1.text = poolOfPowerUps[randomInt1].description.GetLocalizedString();

		namePowerUpText2.text = poolOfPowerUps[randomInt2].powerUpName.GetLocalizedString();
		descriptionPowerUpText2.text = poolOfPowerUps[randomInt2].description.GetLocalizedString();

		namePowerUpText3.text = poolOfPowerUps[randomInt3].powerUpName.GetLocalizedString();
		descriptionPowerUpText3.text = poolOfPowerUps[randomInt3].description.GetLocalizedString();
	}

	private (int, int, int) Get3RandomPowerUps(int numberOfPowerUps)
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

	public void OnPowerUpChosen(int powerUpIndex)
	{
		Debug.Log($"Power up chosen: {powerUpIndex}");
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;

		container.SetActive(false);
	}
}
