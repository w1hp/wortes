using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
//using UnityEngine;


[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct ShootingSystem : ISystem
{
	//private float timer;
	Entity inputEntity;
	InputsData inputsData;
	Entity characterModeEntity;
	CharacterMode characterModeData;

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		inputEntity = SystemAPI.GetSingletonEntity<InputsData>();
		inputsData = state.EntityManager.GetComponentData<InputsData>(inputEntity);

		characterModeEntity = SystemAPI.GetSingletonEntity<CharacterMode>();
		characterModeData = state.EntityManager.GetComponentData<CharacterMode>(characterModeEntity);

		if (characterModeData.mode == Mode.Shoot)
		{
			if (inputsData.action)
			{
				//timer -= SystemAPI.Time.DeltaTime;
				//if (timer > 0)
				//{
				//	return;
				//}
				//timer = 0.3f;   // reset timer

				var config = SystemAPI.GetSingleton<Config>();

				var ballTransform = state.EntityManager.GetComponentData<LocalTransform>(config.CannonBallPrefab);

				foreach (var (gun, transform) in SystemAPI.Query<RefRO<Gun>, RefRO<LocalToWorld>>())
				{
					Entity projectileEntity = state.EntityManager.Instantiate(config.CannonBallPrefab);

					var barrelTransform = state.EntityManager.GetComponentData<LocalToWorld>(gun.ValueRO.Barrel);
					ballTransform.Position = barrelTransform.Position;

					state.EntityManager.SetComponentData(projectileEntity, ballTransform);

					state.EntityManager.SetComponentData(projectileEntity, new Projectile
					{
						Velocity = math.normalize(barrelTransform.Up) * 12.0f
					});
				}
			}
		}
	}
}
