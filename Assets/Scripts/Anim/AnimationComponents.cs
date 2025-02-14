using Unity.Entities;
using UnityEngine;

public class AnimatedGameObjectPrefab : IComponentData
{
	public GameObject Value;
}

public class AnimatorReference : ICleanupComponentData
{
	public Animator Value;
}

//public struct MoveInput : IComponentData
//{
//	public float Value;
//}