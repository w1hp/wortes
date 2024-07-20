using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
	public ElementType enemyType;
	public float detectionRange = 10f;
	public float damage = 10f;
}

class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
	public override void Bake(EnemyAuthoring authoring)
	{
		var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

		AddComponent(entity, new EnemyComponent
		{
			EnemyType = authoring.enemyType,
			DetectionRange = authoring.detectionRange,
			Damage = authoring.damage
		});
	}
}

public struct EnemyComponent : IComponentData
{
	public ElementType EnemyType;
	public float DetectionRange;
	public float Damage;
}
