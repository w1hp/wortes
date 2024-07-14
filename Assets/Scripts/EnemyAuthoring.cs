using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
	public Element enemyType;
	public float health = 50f;
}

class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
	public override void Bake(EnemyAuthoring authoring)
	{
		var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

		AddComponent(entity, new EnemyTag
		{
			EnemyType = authoring.enemyType,
			Health = authoring.health
		});
	}
}

public struct EnemyTag : IComponentData
{
	public Element EnemyType;
	public float Health;
}
