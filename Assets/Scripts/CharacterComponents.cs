using Unity.Entities;

public struct CharacterAttackStrength : IComponentData
{
	public float Value;
}

public struct CharacterExperiencePoints : IComponentData
{
	public float Value;
}

public struct CharacterHitPoints : IComponentData
{
	public float Value;
}

public struct DamageToCharacter : IComponentData
{
	public float Value;
	public ElementType Type;    
	public Entity OriginCharacter;
}
