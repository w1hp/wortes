using System.Collections.Generic;
using Unity.Entities;

public class EnemyDataContainer : IComponentData
{
    public List<EnemyData> enemies;
}

public struct EnemyData
{
    public int level;
    public Entity prefab;
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

	public Entity resourcePrefab;
	public int resourceAmount;
}