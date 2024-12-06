using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct PlayerReloadUpdateSystem : ISystem
{
	public void OnUpdate(ref SystemState state)
	{
		if (ResourceController.Singleton == null)
			return;
		var recourseController = ResourceController.Singleton;


		foreach (var gun in SystemAPI.Query<RefRO<Gun>>())
		{
			if (gun.ValueRO.OriginCharacterType == OriginCharacterType.Player)
			{
				var reloadPercentage = Mathf.Clamp01((gun.ValueRO.FireInterval - gun.ValueRO.LastShotTime) / gun.ValueRO.FireInterval);

				recourseController.UpdateReloadSlider(reloadPercentage);
			}
		}
	}
}
