using Unity.Entities;
using UnityEngine;

class BossAuthoring : MonoBehaviour
{
	//public uint Seed;
	public float Timer;

	public float Health;
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
				MaxHealth = authoring.Health
			});

		}
	}
}

public struct BossTag : IComponentData { }

