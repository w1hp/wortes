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
	[SerializeField] private UnityEngine.UI.Button button;
	[SerializeField] private GameObject stateUiGameObject;

	[Header("Localization")]
	[SerializeField] private LocalizedString lvlUpTextLocalization;
	[SerializeField] private LocalizedString gameOverTextLocalization;

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
		UnityEngine.Time.timeScale = 0;
#if UNITY_EDITOR
		Debug.Log("timeScale = 0");
#endif
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;

		var currentGold = PlayerPrefs.GetFloat("Gold", 0);
		PlayerPrefs.SetFloat("Gold", gold + currentGold);
		PlayerPrefs.Save();

		container.SetActive(true);
		uiInGame.SetActive(false);
		button.onClick.RemoveAllListeners(); // Remove previous listeners
		if (isVictory)
		{
			descriptionText.text = lvlUpTextLocalization.GetLocalizedString();
			button.onClick.AddListener(() => stateUI.NextLevel());
		}
		else
		{
			descriptionText.text = gameOverTextLocalization.GetLocalizedString();
			button.onClick.AddListener(() => stateUI.EndGame());
		}
	}
}
