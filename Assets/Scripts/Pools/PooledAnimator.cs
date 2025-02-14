using Unity.Entities;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

[RequireComponent(typeof(ParticleSystem))]
public class PooledAnimator : MonoBehaviour
{
//	private IObjectPool<PooledAnimator> pool;

//	//private VisualEffect visualEffect;
//	private ParticleSystem visualEffect;
	
//	//private const int MaxSpawnRate = 50;

//	public IObjectPool<PooledAnimator> Pool
//	{
//		get => pool;
//		set => pool = value;
//	}

//	private void OnEnable()
//	{
////#if UNITY_EDITOR
////		UnityEngine.Debug.Log("OnEnable");
////#endif
//		//visualEffect = GetComponent<VisualEffect>();
//		visualEffect = GetComponent<Animator>();

//		var main = visualEffect.main;
//		main.stopAction = ParticleSystemStopAction.Callback;
//	}

////	private void OnParticleSystemStopped()
////	{
////#if UNITY_EDITOR
////		UnityEngine.Debug.Log("OnParticleSystemStopped");
////#endif
		
////		if (OvnerEntity != Entity.Null)
////		{
////			OvnerEntity = Entity.Null;
////			VfxPool.Instance.RemoveVfx(OvnerEntity);
////		}
////	}

//	public void Deactivate()
//	{
//		pool.Release(this);
	//}

	//public void UpdateSmokeRate(int health)
	//{
	//	if (visualEffect == null || !visualEffect.HasInt("SpawnRate"))
	//		return;

	//	visualEffect.SetInt("SpawnRate", MaxSpawnRate - health);
	//}
}
