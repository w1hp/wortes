using Unity.Entities;
using UnityEngine;

class PurchasedUpgradesAuthoring : MonoBehaviour
{
	class Baker : Baker<PurchasedUpgradesAuthoring>
	{
		public override void Bake(PurchasedUpgradesAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			AddComponent(entity, new PurchasedUpgrades
			{
				AttackDamage = PlayerPrefs.GetInt("AttackDamage"),
				AttackSpeed = PlayerPrefs.GetInt("AttackSpeed"),
				MaxHealth = PlayerPrefs.GetInt("MaxHealth"),
				MoveSpeed = PlayerPrefs.GetInt("MoveSpeed")
			});
		}
	}
}


public struct PurchasedUpgrades : IComponentData
{
	public int AttackDamage;
	public int AttackSpeed;
	public int MaxHealth;
	public int MoveSpeed;
}