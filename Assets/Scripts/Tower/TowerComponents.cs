using Unity.Entities;

public struct TowerAimer : IComponentData
{
	public bool IsEnemyInRange;
	public float Range;
	public Entity Aimer;
}
public struct TowerCollision : IComponentData
{
	public bool CanBuild;
}

/// <summary>
/// Struct representing the count of towers built by a player.
/// This should exist only once per player.
/// </summary>
public struct TowerBuiltCount : IComponentData
{
	public int Count;
}
public struct TowerBuildTime : IComponentData
{
	public float BuildTimeRemaining;
	public float BuildTime;
}
public struct TowerCost : IComponentData
{
	public float Cost;
}
public struct TowerElement : IComponentData
{
	public ElementType Element;
}
public struct TowerAmmo : IComponentData
{
	public ushort Ammo;	
}
public struct TowerTag : IComponentData { }

