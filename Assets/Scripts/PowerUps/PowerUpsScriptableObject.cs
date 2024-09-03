using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUps")]
public class PowerUpSO : ScriptableObject
{
	public GameObject prefab;

	public float level;
	public Sprite icon;

	[Header("Localization")]
	public LocalizedString name;
	public LocalizedString description;
}
