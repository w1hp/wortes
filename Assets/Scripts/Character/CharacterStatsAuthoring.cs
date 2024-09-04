using Unity.Entities;
using UnityEngine;

class CharacterStatsAuthoring : MonoBehaviour
{
    
}

class CharacterStatsAuthoringBaker : Baker<CharacterStatsAuthoring>
{
    public override void Bake(CharacterStatsAuthoring authoring)
    {
		var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

		AddComponent<CharacterStats>(entity);


	}
}

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