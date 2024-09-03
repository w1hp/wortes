using UnityEngine;
using UnityEngine.Localization;
using Unity.Entities;


[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUps")]
public class PowerUpSO : ScriptableObject //, IComponentData
{
	public GameObject prefab;

	public float level;
	public Sprite icon;

	[Header("Localization")]
	public LocalizedString powerUpName;
	public LocalizedString description;
}
