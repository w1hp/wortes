using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using System.Collections.Generic;
using TMPro;

public class LevelUpController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject container;
	[SerializeField] private TextMeshProUGUI levelUpText;

	[SerializeField] private TextMeshProUGUI namePowerUp1;
	[SerializeField] private TextMeshProUGUI descriptionPowerUp1;
	[SerializeField] private Image iconPowerUp1;

	[SerializeField] private TextMeshProUGUI namePowerUp2;
	[SerializeField] private TextMeshProUGUI descriptionPowerUp2;
	[SerializeField] private Image iconPowerUp2;

	[SerializeField] private TextMeshProUGUI namePowerUp3;
	[SerializeField] private TextMeshProUGUI descriptionPowerUp3;
	[SerializeField] private Image iconPowerUp3;

	[SerializeField] private List<PowerUpSO> poolOfPowerUps;

	private LevelUpSystem levelUpSystem;

	private (int, int, int) lastGeneratedNumbers;

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

		SetPowerUpData(namePowerUp1, descriptionPowerUp1, iconPowerUp1, randomInt1);
		SetPowerUpData(namePowerUp2, descriptionPowerUp2, iconPowerUp2, randomInt2);
		SetPowerUpData(namePowerUp3, descriptionPowerUp3, iconPowerUp3, randomInt3);
	}

	private void SetPowerUpData(TextMeshProUGUI nameText, TextMeshProUGUI descriptionText, Image iconImage, int powerUpIndex)
	{
		nameText.text = poolOfPowerUps[powerUpIndex].powerUpName.GetLocalizedString();
		descriptionText.text = poolOfPowerUps[powerUpIndex].description.GetLocalizedString();
		iconImage.sprite = poolOfPowerUps[powerUpIndex].icon;
	}

	private (int, int, int) Get3RandomPowerUps(int numberOfPowerUps)
	{
		int randomInt1, randomInt2, randomInt3;
		do
		{
			randomInt1 = Random.Range(0, numberOfPowerUps);
			randomInt2 = Random.Range(0, numberOfPowerUps);
			randomInt3 = Random.Range(0, numberOfPowerUps);
		}
		while (randomInt1 == randomInt2 || randomInt1 == randomInt3 || randomInt2 == randomInt3);

#if UNITY_EDITOR
		Debug.Log($"Generated numbers: {randomInt1}, {randomInt2}, {randomInt3}");
#endif
		return lastGeneratedNumbers = (randomInt1, randomInt2, randomInt3);
	}

	public void OnPowerUpChosen(int powerUpIndex)
	{
		PowerUpSO chosenPowerUp = null;

		switch (powerUpIndex)
		{
			case 0:
				chosenPowerUp = poolOfPowerUps[lastGeneratedNumbers.Item1];
				break;
			case 1:
				chosenPowerUp = poolOfPowerUps[lastGeneratedNumbers.Item2];
				break;
			case 2:
				chosenPowerUp = poolOfPowerUps[lastGeneratedNumbers.Item3];
				break;
		}

		if (chosenPowerUp != null)
		{
#if UNITY_EDITOR
			Debug.Log($"Power up chosen: {chosenPowerUp.name}");
#endif
			levelUpSystem.ReturnToGame(chosenPowerUp.powerUpType, chosenPowerUp.valueInPercent);
		}

		container.SetActive(false);
	}
}
