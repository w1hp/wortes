using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
class MeleeAttackAuthoring : MonoBehaviour
{
	public float radius = 1;
	public ElementType elementType = ElementType.Unidentified;
	public OriginCharacterType originCharacterType = OriginCharacterType.Unidentified;
	public float attackDamage = 1;

	[Header("Debug")]
	public Vector3 positionOverlap = new Vector3(0, 1, 1);


	class Baker : Baker<MeleeAttackAuthoring>
	{
		public override void Bake(MeleeAttackAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new MeleeAttackComponent
			{
				Radius = authoring.radius,
				ElementType = authoring.elementType,
				OriginCharacterType = authoring.originCharacterType,
				AttackDamage = authoring.attackDamage,
				PositionOverlap = authoring.positionOverlap,
			});

		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		//Gizmos.DrawWireSphere(transform.position + transform.forward * 2, radius);
		//Gizmos.DrawWireSphere(transform.position + transform.forward, radius * .75f);
		Gizmos.DrawWireSphere(transform.position + positionOverlap, radius);
	}
}


public struct MeleeAttackComponent : IComponentData
{
	public float Radius;
	public ElementType ElementType;
	public OriginCharacterType OriginCharacterType;
	public float AttackDamage;

	public float3 PositionOverlap;
	
	public float AttackRange;
	public float AttackCooldown;
}