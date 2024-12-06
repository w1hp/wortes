using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scenes;
using Unity.Entities;
using TMPro;
using Unity.Collections;


public class StateUI : MonoBehaviour
{
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject inGameUI;
	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private GameObject settingsPanel;
	[SerializeField] private GameObject introductionPanel;
	[SerializeField] private GameObject mainMenuPanel;
	[SerializeField] private GameObject lvlSelectionPanel;

	[SerializeField] private TextMeshProUGUI goldText;
	[SerializeField] private TextMeshProUGUI purchasedUpgradesHealth;
	[SerializeField] private TextMeshProUGUI purchasedUpgradesDamage;
	[SerializeField] private TextMeshProUGUI purchasedUpgradesMoveSpeed;
	//[SerializeField] private TextMeshProUGUI purchasedUpgradesAttackSpeed;
	[SerializeField] private TextMeshProUGUI purchasedUpgradesBuildSpeed;

	[SerializeField] public GameObject loadingPanel;


	public static StateUI Singleton;
	public int LastSceneIndex { get; private set; }

	private LoadingAction lastClickedAction = LoadingAction.None;
	private bool clicked = false;
	private GameObject currentPanel;

	// Start is called before the first frame update
	void Start()
	{
		Singleton = this;
		currentPanel = mainMenuPanel;
	}

	void Update()
	{
		if (goldText.IsActive())
		{
			goldText.text = PlayerPrefs.GetFloat("Gold", 0).ToString();
			purchasedUpgradesDamage.text = PlayerPrefs.GetInt("AttackDamage", 0).ToString();
			purchasedUpgradesHealth.text = PlayerPrefs.GetInt("MaxHealth", 0).ToString();
			purchasedUpgradesMoveSpeed.text = PlayerPrefs.GetInt("MoveSpeed", 0).ToString();
			//purchasedUpgradesAttackSpeed.text = PlayerPrefs.GetInt("AttackSpeed", 0).ToString();
			purchasedUpgradesBuildSpeed.text = PlayerPrefs.GetInt("BuildSpeed", 0).ToString();
		}
	}

	public bool GetAction(out int sceneIndex, out LoadingAction action)
	{
		sceneIndex = LastSceneIndex;
		action = lastClickedAction;

		var temp = clicked;
		clicked = false;
		return temp;
	}

	public void LoadScene(int sceneIndex)
	{
#if UNITY_EDITOR
		Debug.Log($"lastSceneIndex: {sceneIndex}");
#endif
		LastSceneIndex = sceneIndex;
		lastClickedAction = LoadingAction.LoadAll;
		clicked = true;
		inGameUI.SetActive(true);
		loadingPanel.SetActive(true);
		mainMenu.SetActive(false);
	}
	public void BuyUpgrade(string name)
	{
		if (PlayerPrefs.GetFloat("Gold", 0) >= 100)
		{
			PlayerPrefs.SetFloat("Gold", PlayerPrefs.GetFloat("Gold", 0) - 100);
			switch (name)
			{
				case "AttackDamage":
					PlayerPrefs.SetInt("AttackDamage", PlayerPrefs.GetInt("AttackDamage", 0) + 1);
					break;
				case "MaxHealth":
					PlayerPrefs.SetInt("MaxHealth", PlayerPrefs.GetInt("MaxHealth", 0) + 1);
					break;
				case "MoveSpeed":
					PlayerPrefs.SetInt("MoveSpeed", PlayerPrefs.GetInt("MoveSpeed", 0) + 1);
					break;
				case "BuildSpeed":
					PlayerPrefs.SetInt("BuildSpeed", PlayerPrefs.GetInt("BuildSpeed", 0) + 1);
					break;
			}
			
		}
	}

	public void ShowPanel(GameObject panel)
	{
#if UNITY_EDITOR
		Debug.Log($"Show panel: {panel.name}");
#endif
		currentPanel.SetActive(false);
		panel.SetActive(true);
		currentPanel = panel;
	}

	public void EndGame()
	{
		PauseMenuController.Singleton.SetPause(false, true);

		lastClickedAction = LoadingAction.UnloadAll;
		clicked = true;
		gameOverPanel.SetActive(false);

		mainMenu.SetActive(true);
	}

	public void NextLevel()
	{
		PauseMenuController.Singleton.SetPause(false);
		LastSceneIndex++;
		if (LastSceneIndex > 5)
		{
			EndGame();
			ShowWinPanel();
		}
		else
			LoadScene(LastSceneIndex);

		gameOverPanel.SetActive(false);
	}

	public void ShowWinPanel()
	{
#if UNITY_EDITOR
		Debug.Log("oh wow, you Win");
#endif
	}

	public void UnloadScene()
	{
		loadingPanel.SetActive(true);
		lastClickedAction = LoadingAction.UnloadAll;
		clicked = true;
	}

	public void QuitGame()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
