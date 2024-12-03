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
    [SerializeField] private GameObject loadingPanel;
	[SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject lvlSelectionPanel;
	[SerializeField] private TextMeshProUGUI goldText;

	public static StateUI Singleton;

	//private LevelUpSystem levelUpSystem;


	private LoadingAction lastClickedAction = LoadingAction.None;
	private int lastClickedRow;
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
		sceneIndex = lastClickedRow;
		action = lastClickedAction;

		var temp = clicked;
		clicked = false;
		return temp;
	}


	public void OnActionClick(int sceneIndex)
	{
#if UNITY_EDITOR
		Debug.Log($"OnActionClick: {sceneIndex}");
#endif
		lastClickedRow = sceneIndex;
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
		lastClickedAction = LoadingAction.UnloadAll;
		clicked = true;
		gameOverPanel.SetActive(false);
		mainMenu.SetActive(true);
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
