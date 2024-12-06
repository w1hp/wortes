using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class CharacterStatsAuthoring : MonoBehaviour
{
	public int AttackDamage;
	public int AttackSpeed;
	public int MaxHealth;
	public int MoveSpeed;
	class Baker : Baker<CharacterStatsAuthoring>
	{
		public override void Bake(CharacterStatsAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			AddComponent(entity, new BaseStats
			{
				AttackDamage = authoring.AttackDamage,
				AttackSpeed = authoring.AttackSpeed,
				MaxHealth = authoring.MaxHealth,
				MoveSpeed = authoring.MoveSpeed
			});
			AddComponent(entity, new TotalAttackDamage { Value = authoring.AttackDamage});
			AddComponent(entity, new TotalAttackSpeed{ Value = authoring.AttackSpeed});
			AddComponent(entity, new TotalMoveSpeed{ Value = authoring.MoveSpeed});

		}
	}
}
public struct BaseStats : IComponentData
{
	public int AttackDamage;
	public int AttackSpeed;
	//public int AttackRange;
	//public int CriticalChances;
	//public int CriticalDamage;
	//public int Multishot;
	//int EnemyRecoil;

	public int MaxHealth;
	//int HealthRegeneration;
	//public int DamageReduction;
	//public int DamageBlocking;
	
	public int MoveSpeed;
	//public int PickUpRange;
	//int ExperienceGainMultip;
}


public struct TotalAttackDamage : IComponentData
{
	public float Value;
}

public struct TotalAttackSpeed : IComponentData
{
	public float Value;
}

public struct TotalMoveSpeed : IComponentData
{
	public float Value;
}