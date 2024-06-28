using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct ShootingSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var character in SystemAPI.Query<RefRO<CharacterComponent>>().WithNone<Prefab>())
		{
			if (!character.ValueRO.IsShootMode)
			{
				return;
			}
			var gunComponent = state.EntityManager.GetComponentData<Gun>(character.ValueRO.GunPrefabEntity);
			var bulletTransform = state.EntityManager.GetComponentData<LocalTransform>(character.ValueRO.GunPrefabEntity);
			Entity projectileEntity = state.EntityManager.Instantiate(gunComponent.Bullet);
			var muzzleTransform = state.EntityManager.GetComponentData<LocalToWorld>(gunComponent.Muzzle);
			bulletTransform.Position = muzzleTransform.Position;

			state.EntityManager.SetComponentData(projectileEntity, bulletTransform);

			state.EntityManager.SetComponentData(projectileEntity, new Projectile
			{
				Velocity = math.normalize(muzzleTransform.Up) * 12.0f
			});
		}
	}

}
