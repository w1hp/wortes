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

