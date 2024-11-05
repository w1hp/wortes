using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
			CharacterStatsLookup = SystemAPI.GetComponentLookup<CharacterStats>(),
			TowerAimerLookup = SystemAPI.GetComponentLookup<TowerAimer>(),
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
		[ReadOnly] public ComponentLookup<CharacterStats> CharacterStatsLookup;
		[ReadOnly] public ComponentLookup<TowerAimer> TowerAimerLookup;
		public ComponentLookup<TowerAmmo> TowerAmmoLookup;

		public void Execute(
			ref Gun gun,
			in LocalTransform gunLocalTransform,
			in LocalToWorld gunTransform)
		{
			float damage = gun.Damage;
			switch (gun.OwnerType)
			{
				case GunOwner.Unidentified:
					break;
				case GunOwner.Player:
					var character = CharacterComponentLookup.GetRefRO(gun.Owner);
					if (!character.ValueRO.IsShooting) return;

					var stats = CharacterStatsLookup.GetRefRO(gun.Owner);
					damage += stats.ValueRO.BaseDamage;
					break;
				case GunOwner.Tower:
					var tower = TowerAimerLookup.GetRefRO(gun.Owner);
					if (!tower.ValueRO.IsEnemyInRange) return;

					break;
				case GunOwner.Enemy:
					break;
				default:
					break;
			}

			gun.LastShotTime -= DeltaTime;
			if (gun.LastShotTime > 0) return;
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
				OriginCharacter = gun.Owner
			});

			PhysicsVelocity velocity = new PhysicsVelocity
			{
				Linear = gunTransform.Forward * gun.Strength,
				Angular = float3.zero
			};

			ECB.SetComponent(projectileEntity, velocity);


			if (gun.OwnerType == GunOwner.Tower)
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
