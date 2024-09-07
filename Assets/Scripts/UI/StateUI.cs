using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scenes;
using Unity.Entities;


public class StateUI : MonoBehaviour
{
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject inGame;
	[SerializeField] private GameObject settingsPanel;
	//[SerializeField] private GameObject loadingPanel;
	[SerializeField] private GameObject mainMenuPanel;
	[SerializeField] private GameObject lvlSelectionPanel;

	public static StateUI Singleton;

	private LevelUpSystem levelUpSystem;


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
		lastClickedRow = sceneIndex;
		lastClickedAction = LoadingAction.LoadAll;
		clicked = true;
		inGame.SetActive(true);
		mainMenu.SetActive(false);
	}
	public void ShowPanel(GameObject panel)
	{
		Debug.Log($"Show panel: {panel.name}");
		currentPanel.SetActive(false);
		panel.SetActive(true);
		currentPanel = panel;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	//public void ShowSettings()
	//{
	//	settingsPanel.SetActive(true);
	//	mainMenuPanel.SetActive(false);
	//}
	//public void HideSettings()
	//{
	//	settingsPanel.SetActive(false);
	//	mainMenuPanel.SetActive(true);
	//}
	//public void ShowLevelSelectionPanel()
	//{
	//	lvlSelectionPanel.SetActive(true);
	//	mainMenuPanel.SetActive(false);
	//}
	//public void HideLevelSelectionPanel()
	//{
	//	lvlSelectionPanel.SetActive(false);
	//	mainMenuPanel.SetActive(true);
	//}
}
