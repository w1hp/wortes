using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class CharacterStatsAuthoring : MonoBehaviour
{
	class Baker : Baker<CharacterStatsAuthoring>
	{
		public override void Bake(CharacterStatsAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			AddComponent(entity, new BaseStats());
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

public struct TotalHealth : IComponentData
{
	public float Value;
}


//public partial class InitializePlayerStatsSystem : SystemBase
//{
//	protected override void OnUpdate()
//	{
//		Enabled = false;
//		var ecb = new EntityCommandBuffer(Allocator.TempJob);

//		Entities
//			.WithAll<PlayerTag>()
//			.ForEach((Entity playerEntity, in BaseStats baseStats) =>
//			{
//				ecb.AddComponent(playerEntity, new TotalMoveSpeed { Value = baseStats.MoveSpeed });
//				ecb.AddComponent(playerEntity, new TotalAttackDamage { Value = baseStats.AttackDamage });
//			}).Run();

//		ecb.Playback(EntityManager);
//		ecb.Dispose();
//	}
//}

public struct AttackDamageStatModification : IComponentData
{
	//public StatTypes StatToModify;
	public StatModificationTypes ModificationType;
	public float ModificationValue;
	public float Timer;
}

public struct AttackSpeedStatModification : IComponentData
{
	//public StatTypes StatToModify;
	public StatModificationTypes ModificationType;
	public float ModificationValue;
	public float Timer;
}

public struct MoveSpeedStatModification : IComponentData
{
	//public StatTypes StatToModify;
	public StatModificationTypes ModificationType;
	public float ModificationValue;
	public float Timer;
}

public struct HealthStatModification : IComponentData
{
	//public StatTypes StatToModify;
	public StatModificationTypes ModificationType;
	public float ModificationValue;
	public float Timer;
}

public enum StatTypes
{
	AttackDamage,
	AttackSpeed,
	MoveSpeed,
	MaxHealth,
	None
}

public enum StatModificationTypes
{
	Percentage,
	Numerical,
	Absolute,
	None
}

public struct ModifyStatsTag : IComponentData { }