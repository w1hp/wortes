 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject introductionPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject lvlSelectionPanel;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI purchasedUpgradesHealth;
    [SerializeField] private TextMeshProUGUI purchasedUpgradesDamage;
    [SerializeField] private TextMeshProUGUI purchasedUpgradesMoveSpeed;
    [SerializeField] private TextMeshProUGUI purchasedUpgradesBuildSpeed;

    [SerializeField] public GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI errorMessageText;

    public static StateUI Singleton;
    public int LastSceneIndex { get; private set; }

    private LoadingAction lastClickedAction = LoadingAction.None;
    private bool clicked = false;
    private GameObject currentPanel;

    void Start()
    {
        Singleton = this;

        if (mainMenuPanel == null)
        {
            Debug.LogError("mainMenuPanel nie jest przypisany w inspektorze!");
        }

        if (errorMessageText == null)
        {
            Debug.LogError("errorMessageText nie jest przypisany w inspektorze!");
        }
        else
        {
            errorMessageText.gameObject.SetActive(false);
        }

        currentPanel = mainMenuPanel;
    }


    void Update()
    {
        if (goldText.IsActive())
        {
            goldText.text = PlayerPrefs.GetFloat("Gold", 0).ToString();
            purchasedUpgradesDamage.text = PlayerPrefs.GetInt("AttackDamage", 1).ToString();
            purchasedUpgradesHealth.text = PlayerPrefs.GetInt("MaxHealth", 1).ToString();
            purchasedUpgradesMoveSpeed.text = PlayerPrefs.GetInt("MoveSpeed", 1).ToString();
            purchasedUpgradesBuildSpeed.text = PlayerPrefs.GetInt("BuildSpeed", 1).ToString();
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
        float gold = PlayerPrefs.GetFloat("Gold", 0);
        if (gold >= 100)
        {
            PlayerPrefs.SetFloat("Gold", gold - 100);
            switch (name)
            {
                case "AttackDamage":
                    PlayerPrefs.SetInt("AttackDamage", PlayerPrefs.GetInt("AttackDamage", 1) + 1);
                    break;
                case "MaxHealth":
                    PlayerPrefs.SetInt("MaxHealth", PlayerPrefs.GetInt("MaxHealth", 1) + 1);
                    break;
                case "MoveSpeed":
                    PlayerPrefs.SetInt("MoveSpeed", PlayerPrefs.GetInt("MoveSpeed", 1) + 1);
                    break;
                case "BuildSpeed":
                    PlayerPrefs.SetInt("BuildSpeed", PlayerPrefs.GetInt("BuildSpeed", 1) + 1);
                    break;
            }
        }
        else
        {
            ShowErrorMessage("Masz za mało pieniędzy!");
        }
    }

    private void ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        Invoke(nameof(HideErrorMessage), 2.0f);
    }

    private void HideErrorMessage()
    {
        errorMessageText.gameObject.SetActive(false);
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
