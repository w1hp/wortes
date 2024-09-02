using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Localization;

public class GameOverController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject container;
	[SerializeField] private TextMeshProUGUI gameOverText;
    [Header("Localization")]
    [SerializeField] private LocalizedString lvlUpTextLocalization;
	[SerializeField] private LocalizedString gameOverTextLocalization;

    private GameOverSystem gameOverSystem;

    void Awake()
    {
        gameOverSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameOverSystem>();
	    container.SetActive(false);
	}


	private void OnEnable()
	{
		gameOverSystem.OnGameOver += OnGameOverEvent;
	}

	private void OnDisable()
	{
		gameOverSystem.OnGameOver -= OnGameOverEvent;
	}

	private void OnGameOverEvent(bool isPlayerDead)
	{
		container.SetActive(true);
		gameOverText.text = isPlayerDead
			? gameOverTextLocalization.GetLocalizedString()
			: lvlUpTextLocalization.GetLocalizedString();
	}
}
