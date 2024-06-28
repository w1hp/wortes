using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Physics;

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
		public float DeltaTime;
		public ComponentLookup<CharacterComponent> CharacterComponentLookup;

		public void Execute( 
			ref Gun gun,
			in LocalTransform gunLocalTransform, 
			in LocalToWorld gunTransform)
		{


			if (gun.OwnerType == GunOwner.Player)
			{
				var character = CharacterComponentLookup.GetRefRO(gun.Owner);
				if (!character.ValueRO.IsShooting) return;
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

			PhysicsVelocity velocity = new PhysicsVelocity
			{
				Linear = gunTransform.Forward * gun.Strength,
				Angular = float3.zero
			};
			ECB.SetComponent(projectileEntity, velocity);

			//URPMaterialPropertyBaseColor color
			//ECB.SetComponent(projectileEntity, color);

			//ECB.SetComponent(projectileEntity, new Projectile
			//{
			//	Velocity = math.normalize(gunTransform.Up) * 12.0f
			//});
		}
	}
}
