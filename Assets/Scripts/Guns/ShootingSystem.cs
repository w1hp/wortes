using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Physics;
using Unity.Collections;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct ShootingSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Gun>();
		state.RequireForUpdate<IsNotPause>();
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
			Tower = SystemAPI.GetComponentLookup<Tower>()
		};

		shootingJob.Schedule();

		//foreach (var gun in SystemAPI.Query<RefRW<Gun>>().WithNone<Prefab>())
		//{
		//	if (gun.ValueRO.OwnerType == GunOwner.Player)
		//	{
		//		var character = SystemAPI.GetComponentRO<CharacterComponent>(gun.ValueRO.Owner);
		//		if (!character.ValueRO.IsShooting) break;
		//	}

		//	gun.ValueRW.LastShotTime -= SystemAPI.Time.DeltaTime;
		//	if (gun.ValueRW.LastShotTime > 0)
		//	{
		//		continue;
		//	}
		//	gun.ValueRW.LastShotTime = gun.ValueRO.FireInterval;

		//	Entity projectileEntity = state.EntityManager.Instantiate(gun.ValueRO.Bullet);

		//	var muzzleTransform = state.EntityManager.GetComponentData<LocalToWorld>(gun.ValueRO.Muzzle);
		//	var bulletTransform = state.EntityManager.GetComponentData<LocalTransform>(gun.ValueRO.Bullet);
		//	bulletTransform.Position = muzzleTransform.Position;

		//	state.EntityManager.SetComponentData(projectileEntity, bulletTransform);

		//	// Set color (color is RefRO<URPMaterialPropertyBaseColor> type)
		//	//state.EntityManager.SetComponentData(projectileEntity, color.ValueRO);

		//	state.EntityManager.SetComponentData(projectileEntity, new Projectile
		//	{
		//		Velocity = math.normalize(muzzleTransform.Up) * 12.0f
		//	});
		//}

	}
	[BurstCompile]
	[WithNone(typeof(Prefab))]
	public partial struct ShootingJob : IJobEntity
	{
		public EntityCommandBuffer ECB;
		[ReadOnly] public float DeltaTime;
		[ReadOnly] public ComponentLookup<CharacterComponent> CharacterComponentLookup;
		[ReadOnly] public ComponentLookup<CharacterStats> CharacterStatsLookup;
		[ReadOnly] public ComponentLookup<Tower> Tower;

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
					var tower = Tower.GetRefRO(gun.Owner);
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
				Type = gun.ElementType
			});

			PhysicsVelocity velocity = new PhysicsVelocity
			{
				Linear = gunTransform.Forward * gun.Strength,
				Angular = float3.zero
			};

			ECB.SetComponent(projectileEntity, velocity);
			
			//ECB.SetComponentEnabled<MaterialMeshInfo>(projectileEntity., true);
			//URPMaterialPropertyBaseColor color
			//ECB.SetComponent(projectileEntity, color);

			//ECB.SetComponent(projectileEntity, new Projectile
			//{
			//	Velocity = math.normalize(gunTransform.Up) * 12.0f
			//});
		}
	}

}
