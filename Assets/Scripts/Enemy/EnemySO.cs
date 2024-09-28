using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    public int level;
    public GameObject prefab;
	public float detectionRange;
    public ElementType elementType;
	public float damage;
    public float moveSpeed;

    public float maxHealth;
	public float fireResistance;
	public float waterResistance;
	public float earthResistance;
	public float woodResistance;
	public float metalResistance;

	public float experiencePoints;

	public GameObject resourcePrefab;
	public int resourceAmount;
	//public HealthBar healthBarData;
}