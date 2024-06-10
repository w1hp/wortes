using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct ShootingSystem : ISystem
{
	//private float timer;
	private float lastShotTime;

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		// Only shoot in frames where timer has expired
		//timer -= SystemAPI.Time.DeltaTime;
		//if (timer > 0)
		//{
		//	return;
		//}
		//timer = 0.3f;   // reset timer

		foreach (var gun in SystemAPI.Query<RefRO<Gun>>().WithNone<Prefab>())
		{
			if (gun.ValueRO.OwnerType == GunOwner.Player)
			{
				var character = SystemAPI.GetComponentRO<CharacterComponent>(gun.ValueRO.Owner);
				if (!character.ValueRO.IsShootMode) break;
			}
			if (!(SystemAPI.Time.ElapsedTime > lastShotTime + gun.ValueRO.FireInterval)) break;

			lastShotTime = (float)SystemAPI.Time.ElapsedTime;

			Entity projectileEntity = state.EntityManager.Instantiate(gun.ValueRO.Bullet);

			var muzzleTransform = state.EntityManager.GetComponentData<LocalToWorld>(gun.ValueRO.Muzzle);
			var bulletTransform = state.EntityManager.GetComponentData<LocalTransform>(gun.ValueRO.Bullet);
			bulletTransform.Position = muzzleTransform.Position;

			state.EntityManager.SetComponentData(projectileEntity, bulletTransform);

			// Set color (color is RefRO<URPMaterialPropertyBaseColor> type)
			//state.EntityManager.SetComponentData(projectileEntity, color.ValueRO);

			state.EntityManager.SetComponentData(projectileEntity, new Projectile
			{
				Velocity = math.normalize(muzzleTransform.Up) * 12.0f
			});
		}

	}
}
