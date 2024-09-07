using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class MainCameraSystem : SystemBase
{
	protected override void OnCreate()
	{
        RequireForUpdate<IsNotPause>();
	}
	protected override void OnUpdate()
    {
        if (MainGameObjectCamera.Instance != null && SystemAPI.HasSingleton<MainEntityCamera>())
        {
            Entity mainEntityCameraEntity = SystemAPI.GetSingletonEntity<MainEntityCamera>();
            LocalToWorld targetLocalToWorld = SystemAPI.GetComponent<LocalToWorld>(mainEntityCameraEntity);
			MainGameObjectCamera.Instance.transform.SetPositionAndRotation(targetLocalToWorld.Position, targetLocalToWorld.Rotation);
			
			//float3 newPosition = targetLocalToWorld.Position;
			//var newRotation = targetLocalToWorld.Rotation;
			//quaternion newRotation = quaternion.Euler(90, 0, 0);
			//newPosition -= 10.0f * targetLocalToWorld.Forward;
			//newPosition += new float3(0, 10f, 0);
			//MainGameObjectCamera.Instance.transform.SetPositionAndRotation(
   //             newPosition,
			//	newRotation);



		}
    }
}