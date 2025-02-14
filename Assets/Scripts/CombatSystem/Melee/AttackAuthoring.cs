using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
class AttackAuthoring : MonoBehaviour
{
	//public float radius = 1;
	public ElementType elementType = ElementType.Unidentified;
	public OriginCharacterType originCharacterType = OriginCharacterType.Unidentified;
	public float attackDamage = 1;
	public float attackCooldown = 1;
	public Vector3 attackRange = new Vector3(1, 1, 1);

	public GameObject GraphicsRepresentation;

	public bool DrawGizmos;


	class Baker : Baker<AttackAuthoring>
	{
		public override void Bake(AttackAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new AttackComponent
			{
				//Radius = authoring.radius,
				ElementType = authoring.elementType,
				OriginCharacterType = authoring.originCharacterType,
				AttackDamage = authoring.attackDamage,
				AttackCooldown = authoring.attackCooldown,
				AttackRange = authoring.attackRange,
				GraphicsRepresentation = GetEntity(authoring.GraphicsRepresentation, TransformUsageFlags.Dynamic)
			});

		}
	}

	private void OnDrawGizmos()
	{
		if (!DrawGizmos)
			return;
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position, attackRange*2);
	}
}


public struct AttackComponent : IComponentData
{
	//public float Radius;
	public ElementType ElementType;
	public OriginCharacterType OriginCharacterType;
	public float AttackDamage;
	public float AttackCooldown;
	public float LastAttackTime;
	public float3 AttackRange;

	public Entity GraphicsRepresentation;
}