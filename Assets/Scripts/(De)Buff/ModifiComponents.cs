using Unity.Entities;

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