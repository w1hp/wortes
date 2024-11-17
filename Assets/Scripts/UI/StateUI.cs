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
	[SerializeField] private GameObject mainMenuPanel;
	[SerializeField] private GameObject lvlSelectionPanel;
	[SerializeField] private TextMeshProUGUI goldText;

	public GameObject loadingPanel;
	public static StateUI Singleton;

	//private LevelUpSystem levelUpSystem;


	private LoadingAction lastClickedAction = LoadingAction.None;
	private int lastSceneIndex;
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
			goldText.text = PlayerPrefs.GetFloat("Gold", 0).ToString();
	}



	public bool GetAction(out int sceneIndex, out LoadingAction action)
	{
		sceneIndex = lastSceneIndex;
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
		lastSceneIndex = sceneIndex;
		lastClickedAction = LoadingAction.LoadAll;
		clicked = true;
		inGameUI.SetActive(true);
		loadingPanel.SetActive(true);
		mainMenu.SetActive(false);
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
		UnityEngine.Time.timeScale = 1;
#if UNITY_EDITOR
		Debug.Log("timeScale = 1");
#endif
		lastClickedAction = LoadingAction.UnloadAll;
		clicked = true;
		gameOverPanel.SetActive(false);

		mainMenu.SetActive(true);
	}
	public void NextLevel()
	{
		UnityEngine.Time.timeScale = 1;
#if UNITY_EDITOR
		Debug.Log("timeScale = 1");
#endif
		lastSceneIndex++;
		if (lastSceneIndex > 5)
		{
			EndGame();
			ShowWinPanel();
		}
		else
			LoadScene(lastSceneIndex);

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


	//public void SetPause(bool value)
	//{
	//	UnityEngine.Time.timeScale = value ? 0 : 1;
	//	Cursor.visible = value ? true : false;
	//	Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.Locked;
	//}

	public void QuitGame()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
