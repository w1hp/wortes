using UnityEngine;
using UnityEngine.Localization;
using Unity.Entities;
using TMPro;

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

	private void OnLevelUpEvent(PowerUpSO powerUp1, PowerUpSO powerUp2, PowerUpSO powerUp)
	{
		container.SetActive(true);
		namePowerUpText1.text = powerUp1.name.GetLocalizedString();
		descriptionPowerUpText1.text = powerUp1.description.GetLocalizedString();

		namePowerUpText2.text = powerUp2.name.GetLocalizedString();
		descriptionPowerUpText2.text = powerUp2.description.GetLocalizedString();

		namePowerUpText3.text = powerUp.name.GetLocalizedString();
		descriptionPowerUpText3.text = powerUp.description.GetLocalizedString();
	}
}
