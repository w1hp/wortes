using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class WorldSpaceUIController : MonoBehaviour
{
	[SerializeField] private GameObject _damageIconPrefab;

	private Transform _mainCameraTransform;

	private void Start()
	{
		_mainCameraTransform = Camera.main.transform;
	}

	private void OnEnable()
	{
		var dealDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DealDamageSystem>();
		dealDamageSystem.OnDealDamage += DisplayDamageIcon;
		//dealDamageSystem.OnGrantExperience += DisplayExperienceIcon;
	}

	private void OnDisable()
	{
		if (World.DefaultGameObjectInjectionWorld == null) return;
		var dealDamageSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DealDamageSystem>();
		dealDamageSystem.OnDealDamage -= DisplayDamageIcon;
		//dealDamageSystem.OnGrantExperience -= DisplayExperienceIcon;
	}

	private void DisplayDamageIcon(float damageAmount, float3 startPosition, AttackResult attackResult)
	{
		if (attackResult == AttackResult.NoEffect) return;
		var directionToCamera = (Vector3)startPosition - _mainCameraTransform.position;
		var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
		var newIcon = Instantiate(_damageIconPrefab, startPosition, rotationToCamera, transform);
		var newIconText = newIcon.GetComponent<TextMeshProUGUI>();
		switch (attackResult)
		{
			case AttackResult.Healed:
				newIconText.text = $"<color=green>+{damageAmount.ToString()}</color>";
				break;
			case AttackResult.Damaged:
				newIconText.text = $"<color=red>-{damageAmount.ToString()}</color>";
				break;
		}
	}

	//private void DisplayExperienceIcon(float experienceAmount, float3 startPosition)
	//{
	//	var directionToCamera = (Vector3)startPosition - _mainCameraTransform.position;
	//	var rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);
	//	var newIcon = Instantiate(_damageIconPrefab, startPosition, rotationToCamera, transform);
	//	var newIconText = newIcon.GetComponent<TextMeshProUGUI>();
	//	newIconText.text = $"<color=yellow>+{experienceAmount.ToString()} EXP</color>";
	//}
}
