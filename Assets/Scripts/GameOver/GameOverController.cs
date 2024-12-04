using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Localization;


public class GameOverController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject container;
	[SerializeField] private GameObject uiInGame;
	[SerializeField] private TextMeshProUGUI descriptionText;
	[SerializeField] private TextMeshProUGUI buttonText;
	[SerializeField] private UnityEngine.UI.Button button;
	[SerializeField] private GameObject stateUiGameObject;

	[Header("Localization")]
	[SerializeField] private LocalizedString lvlUpDescriptionTextLocalization;
	[SerializeField] private LocalizedString gameOverDescriptionTextLocalization;
	[SerializeField] private LocalizedString lvlUpButtonTextLocalization;
	[SerializeField] private LocalizedString gameOverButtonTextLocalization;

	private GameOverSystem gameOverSystem;
	private StateUI stateUI;

	void Awake()
	{
		gameOverSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameOverSystem>();
		container.SetActive(false);
		stateUI = stateUiGameObject.GetComponent<StateUI>();
	}


	private void OnEnable()
	{
		gameOverSystem.OnGameOver += OnGameOverEvent;
	}

	private void OnDisable()
	{
		gameOverSystem.OnGameOver -= OnGameOverEvent;
	}

	private void OnGameOverEvent(bool isVictory, float gold)
	{
		stateUI.UnloadScene();

		PauseMenuController.Singleton.SetPause(true);

		var currentGold = PlayerPrefs.GetFloat("Gold", 0);
		PlayerPrefs.SetFloat("Gold", gold + currentGold);
		PlayerPrefs.Save();


		var countdownController = CountdownController.Singleton;
		countdownController.UpdateBossHealthPanel(false);

		container.SetActive(true);
		uiInGame.SetActive(false);
		button.onClick.RemoveAllListeners();
		if (isVictory)
		{
			descriptionText.text = lvlUpDescriptionTextLocalization.GetLocalizedString();
			buttonText.text = lvlUpButtonTextLocalization.GetLocalizedString();
			button.onClick.AddListener(() => stateUI.NextLevel());
		}
		else
		{
			descriptionText.text = gameOverDescriptionTextLocalization.GetLocalizedString();
			buttonText.text = gameOverButtonTextLocalization.GetLocalizedString();
			button.onClick.AddListener(() => stateUI.EndGame());
		}
	}
}
