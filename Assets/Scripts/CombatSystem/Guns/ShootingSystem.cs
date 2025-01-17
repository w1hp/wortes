using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Collections;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct ShootingSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Gun>();
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

		var shootingJob = new ShootingJob
		{
			ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
			DeltaTime = SystemAPI.Time.DeltaTime,
			CharacterComponentLookup = SystemAPI.GetComponentLookup<CharacterComponent>(),
			CharacterStatsLookup = SystemAPI.GetComponentLookup<BaseStats>(),
			TowerAimerLookup = SystemAPI.GetComponentLookup<TowerAimer>(),
			BossStateMachineLookup = SystemAPI.GetComponentLookup<BossStateMachine>(),
			TowerAmmoLookup = SystemAPI.GetComponentLookup<TowerAmmo>()
		};

		shootingJob.Schedule();
	}
	[BurstCompile]
	[WithNone(typeof(Prefab))]
	public partial struct ShootingJob : IJobEntity
	{
		public EntityCommandBuffer ECB;
		[ReadOnly] public float DeltaTime;
		[ReadOnly] public ComponentLookup<CharacterComponent> CharacterComponentLookup;
		[ReadOnly] public ComponentLookup<BaseStats> CharacterStatsLookup;
		[ReadOnly] public ComponentLookup<TowerAimer> TowerAimerLookup;
		[ReadOnly] public ComponentLookup<BossStateMachine> BossStateMachineLookup;
		public ComponentLookup<TowerAmmo> TowerAmmoLookup;

		public void Execute(
			ref Gun gun,
			in LocalTransform gunLocalTransform,
			in LocalToWorld gunTransform)
		{
			gun.LastShotTime -= DeltaTime;
			if (gun.LastShotTime > 0) return;

			float damage = gun.Damage;
			switch (gun.OriginCharacterType)
			{
				case OriginCharacterType.Unidentified:
#if UNITY_EDITOR
					UnityEngine.Debug.Log("Unidentified character type");
#endif
					return;
				case OriginCharacterType.Player:
					var character = CharacterComponentLookup.GetRefRO(gun.Owner);
					//if (!character.ValueRO.IsShooting) return;

					var stats = CharacterStatsLookup.GetRefRO(gun.Owner);
					damage += stats.ValueRO.AttackDamage;
					break;
				case OriginCharacterType.Tower:
					var tower = TowerAimerLookup.GetRefRO(gun.Owner);
					if (!tower.ValueRO.IsEnemyInRange) return;

					break;
				case OriginCharacterType.Enemy:
					var enemy = BossStateMachineLookup.GetRefRO(gun.Owner);
					if (enemy.ValueRO.CurrentState != BossState.Attack) return;
					break;
				default:
					break;
			}


			gun.LastShotTime = gun.FireInterval;

			Entity projectileEntity = ECB.Instantiate(gun.Bullet);

			LocalTransform localTransform = LocalTransform.FromPositionRotationScale(
				gunTransform.Position + gunTransform.Forward,
				gunLocalTransform.Rotation,
				gunLocalTransform.Scale);
			ECB.SetComponent(projectileEntity, localTransform);

			ECB.SetComponent(projectileEntity, new Projectile
			{
				Damage = damage,
				Type = gun.ElementType,
				OriginCharacterType = gun.OriginCharacterType
			});

			PhysicsVelocity velocity = new PhysicsVelocity
			{
				Linear = gunTransform.Forward * gun.Strength,
				Angular = float3.zero
			};

			ECB.SetComponent(projectileEntity, velocity);


			if (gun.OriginCharacterType == OriginCharacterType.Tower)
			{
				var ammo = TowerAmmoLookup.GetRefRW(gun.Owner);
				ammo.ValueRW.Ammo--;
				if (ammo.ValueRO.Ammo <= 0)
				{
					ECB.SetComponentEnabled<IsExistTag>(gun.Owner, false);
				}
			}
		}
	}

}
