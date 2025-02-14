using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
	public ElementType enemyType;
	public float detectionRange = 100f;
	public float damage = 10f;
    public float moveSpeed = 2f;
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
			Damage = authoring.damage,
			moveSpeed = authoring.moveSpeed
		});
	}
}

public struct EnemyComponent : IComponentData
{
	public ElementType EnemyType;
	public float DetectionRange;
	public float Damage;
    public float moveSpeed;
	public float MoveInput;
}
