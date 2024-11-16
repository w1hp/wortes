using Unity.Entities;
using UnityEngine;

class BossAuthoring : MonoBehaviour
{
	public float Timer;

	public float Health;
	public float FireResistance;
	public float WaterResistance;
	public float EarthResistance;
	public float MetalResistance;
	public float WoodResistance;
	class Baker : Baker<BossAuthoring>
	{
		public override void Bake(BossAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<BossTag>(entity);
			AddComponent(entity, new BossStateMachine
			{
				CurrentState = BossState.Idle,
				PreviousState = BossState.Attack,
				Timer = authoring.Timer,
				//Seed = authoring.Seed
			});
			AddComponent(entity, new Health
			{
				CurrentHealth = authoring.Health,
				MaxHealth = authoring.Health,
				FireResistance = authoring.FireResistance,
				WaterResistance = authoring.WaterResistance,
				EarthResistance = authoring.EarthResistance,
				WoodResistance = authoring.WoodResistance,
				MetalResistance = authoring.MetalResistance
			});
			AddComponent<EnemyTag>(entity);
			AddComponent<IsExistTag>(entity);
		}
	}
}

public struct BossTag : IComponentData { }

