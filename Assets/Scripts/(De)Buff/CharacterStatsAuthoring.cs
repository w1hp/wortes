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

			AddComponent<CharacterStats>(entity);
		}
	}
}


//TODO: check if this is the correct way to implement the CharacterStats component
public struct CharacterStats : IComponentData
{
	public int BaseDamage;
	public int AttackSpeed;
	public int AttackRange;
	public int CriticalChances;
	public int CriticalDamage;
	public int Multishot;
	//int EnemyRecoil;

	public int MaxHealth;
	//int HealthRegeneration;
	public int DamageReduction;
	public int DamageBlocking;
	
	public int MovementSpeed;
	public int PickUpRange;
	//int ExperienceGainMultip;
}
public struct BaseStats : IComponentData
{
	public float AttackDamage;
	public float MoveSpeed;
}

public struct TotalAttackDamage : IComponentData
{
	public float Value;
}

public struct TotalMoveSpeed : IComponentData
{
	public float Value;
}
public struct Damage : IComponentData
{
	public float Value;
}

public partial class InitializePlayerStatsSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Enabled = false;
		var ecb = new EntityCommandBuffer(Allocator.TempJob);

		Entities
			.WithAll<PlayerTag>()
			.ForEach((Entity playerEntity, in BaseStats baseStats) =>
			{
				ecb.AddComponent(playerEntity, new TotalMoveSpeed { Value = baseStats.MoveSpeed });
				ecb.AddComponent(playerEntity, new TotalAttackDamage { Value = baseStats.AttackDamage });
			}).Run();

		ecb.Playback(EntityManager);
		ecb.Dispose();
	}
}

public struct StatModification : IComponentData
{
	public StatTypes StatToModify;
	public StatModificationTypes ModificationType;
	public float ModificationValue;
	public float Timer;

	public static StatModification Empty => new StatModification
	{
		StatToModify = StatTypes.None,
		ModificationType = StatModificationTypes.None,
		ModificationValue = 0f,
		Timer = 0f
	};
}

public enum StatTypes
{
	AttackDamage,
	MoveSpeed,
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