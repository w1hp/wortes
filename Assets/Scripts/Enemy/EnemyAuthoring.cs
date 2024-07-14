using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
	public ElementType enemyType;
	public float currentHealth = 50f;
	public float detectionRange = 10f;
}

class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
	public override void Bake(EnemyAuthoring authoring)
	{
		var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

		AddComponent(entity, new EnemyComponent
		{
			EnemyType = authoring.enemyType,
			CurrentHealth = authoring.currentHealth,
			DetectionRange = authoring.detectionRange
		});
	}
}

public struct EnemyComponent : IComponentData
{
	public ElementType EnemyType;
	public float CurrentHealth;
	public float DetectionRange;

	public void TakeDamage(float amount, ElementType typeDamage)
	{
		switch (EnemyType)
		{ 
			case ElementType.Fire:
				if (typeDamage == ElementType.Water)
				{
					CurrentHealth -= amount;
				}
				else if (typeDamage == ElementType.Fire)
				{
					CurrentHealth += amount * 0.5f;
				}
				break;
			case ElementType.Water:
				if (typeDamage == ElementType.Earth)
				{
					CurrentHealth -= amount;
				}
				else if (typeDamage == ElementType.Water)
				{
					CurrentHealth += amount * 0.5f;
				}
				break;
			case ElementType.Earth:
				if (typeDamage == ElementType.Wood)
				{
					CurrentHealth -= amount;
				}
				else if (typeDamage == ElementType.Earth)
				{
					CurrentHealth += amount * 0.5f;
				}
				break;
			case ElementType.Wood:
				if (typeDamage == ElementType.Metal)
				{
					CurrentHealth -= amount;
				}
				else if (typeDamage == ElementType.Wood)
				{
					CurrentHealth += amount * 0.5f;
				}
				break;
			case ElementType.Metal:
				if (typeDamage == ElementType.Fire)
				{
					CurrentHealth -= amount;
				}
				else if (typeDamage == ElementType.Metal)
				{
					CurrentHealth += amount * 0.5f;
				}
				break;
		}
	}
}
