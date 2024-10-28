using System;
using System.Collections;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceUIController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _playerHealthText;
	[SerializeField] private Slider _playerHealthSlider;

	[SerializeField] private Text _woodText;
	[SerializeField] private Text _fireText;
	[SerializeField] private Text _waterText;
	[SerializeField] private Text _earthText;
	[SerializeField] private Text _metalText;

	private Entity _playerEntity;
	private EntityManager _entityManager;
	private bool isInitPlayer;

	private IEnumerator Initialize()
	{
		_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		while (!isInitPlayer)
		{
			isInitPlayer = _entityManager.CreateEntityQuery(typeof(PlayerTag)).TryGetSingletonEntity<PlayerTag>(out _playerEntity);
#if UNITY_EDITOR
			Debug.Log($"Player is not initialized, wait 1s");
#endif
			yield return new WaitForSeconds(1f);
		}

#if UNITY_EDITOR
		Debug.Log($"Player: {_playerEntity.ToString()}");
#endif
	}

	private void OnEnable()
	{
		StartCoroutine(Initialize());
	}

	private void OnDisable()
	{
		_entityManager = default;
		_playerEntity = default;
		isInitPlayer = default;
	}

	private void Update()
	{
		if (_entityManager.Exists(_playerEntity))
		{
			var playerHealth = _entityManager.GetComponentData<Health>(_playerEntity);
			var playerInventory = _entityManager.GetComponentData<Inventory>(_playerEntity);

			_playerHealthSlider.maxValue = playerHealth.MaxHealth;
			_playerHealthSlider.value = playerHealth.CurrentHealth;
			_playerHealthText.text = $"Player HP: {playerHealth.CurrentHealth}";

			_woodText.text = $"{playerInventory.Wood}";
			_fireText.text = $"{playerInventory.Fire}";
			_waterText.text = $"{playerInventory.Water}";
			_earthText.text = $"{playerInventory.Earth}";
			_metalText.text = $"{playerInventory.Metal}";
		}
	}
}