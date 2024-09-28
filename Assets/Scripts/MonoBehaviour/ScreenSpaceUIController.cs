using System.Collections;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceUIController : MonoBehaviour
{
	//[SerializeField] private GameObject _statsMenu;
	//[SerializeField] private TextMeshProUGUI _playerExperienceText;
	//[SerializeField] private Slider _playerExperienceSlider;

	//private bool _showStats;
	//private Entity _playerEntity;
	//private EntityManager _entityManager;

	//private IEnumerator Start()
	//{
	//	_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

	//	// Wait a few frames until ECS world is loaded, then get player entity reference
	//	yield return new WaitForSeconds(0.2f);
	//	_playerEntity = _entityManager.CreateEntityQuery(typeof(PlayerTag)).GetSingletonEntity();
	//	Debug.Log($"Player: {_playerEntity.ToString()}");
	//}

	//private void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.Escape))
	//	{
	//		_showStats = !_showStats;
	//		ShowHideStatsMenu();
	//	}
	//}

	//private void ShowHideStatsMenu()
	//{
	//	_statsMenu.SetActive(_showStats);

	//	if (!_showStats) return;

	//	var curPlayerExperience = _entityManager.GetComponentData<CharacterExperiencePoints>(_playerEntity).Value;
	//	_playerExperienceText.text = $"Player EXP: {curPlayerExperience}";
	//	_playerExperienceSlider.value = curPlayerExperience;
	//}
}