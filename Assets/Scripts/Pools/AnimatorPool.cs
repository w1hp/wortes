using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Pool;

public class AnimatorPool : MonoBehaviour
{
//	public static VfxPool Instance;

//	[SerializeField] private PooledVfx vfxPrefab;

//	[SerializeField] private bool collectionCheck = true;
//	[SerializeField] private int defaultCapacity = 20;
//	[SerializeField] private int maxSize = 100;


//	private Dictionary<Entity, PooledVfx> vfxDictionary = new Dictionary<Entity, PooledVfx>();

//	private IObjectPool<PooledVfx> pool;

//	private void Awake()
//	{
//		if (Instance == null)
//			Instance = this;
//		else
//			Destroy(gameObject);

//		pool = new ObjectPool<PooledVfx>(
//			CreateVfx,
//			OnGetFromPool,
//			OnReleaseToPool,
//			OnDestroyPooledObject,
//			collectionCheck, defaultCapacity, maxSize
//			);
//	}
//	private PooledVfx CreateVfx()
//	{
//		var vfx = Instantiate(vfxPrefab);
//		vfx.Pool = pool;
//		return vfx;
//	}
//	private void OnGetFromPool(PooledVfx vfx)
//	{
//		vfx.gameObject.SetActive(true);
//	}
//	private void OnReleaseToPool(PooledVfx vfx)
//	{
//		vfx.gameObject.SetActive(false);
//	}
//	private void OnDestroyPooledObject(PooledVfx vfx)
//	{
//		Destroy(vfx.gameObject);
//	}


//	public void RemoveVfx(Entity entity)
//	{
//		if (vfxDictionary.TryGetValue(entity, out PooledVfx vfx))
//		{
//			vfxDictionary.Remove(entity);
//			pool.Release(vfx);
//		}
//	}
//	public void UpdateVfx(Vector3 position, Quaternion rotation, Entity entity)
//	{
//		if (vfxDictionary.TryGetValue(entity, out PooledVfx vfx))
//		{
//			vfx.transform.SetPositionAndRotation(position, rotation);
//			//vfx.UpdateSmokeRate((int)health);
//		}
//		else
//		{
//#if UNITY_EDITOR
//			UnityEngine.Debug.Log("Get from pool");
//#endif
//			PooledVfx vfxObject = pool.Get();
//			vfxObject.transform.SetPositionAndRotation(position, rotation);
//			vfxDictionary.Add(entity, vfxObject);
//		}
//	}

//	public void ClearMissingEntities(EntityManager entityManager)
//	{
//		List<Entity> nullEntities = new List<Entity>();
//		foreach (KeyValuePair<Entity, PooledVfx> item in vfxDictionary)
//		{
//			if (!entityManager.Exists(item.Key))
//			{
//				item.Value.Deactivate();
//				nullEntities.Add(item.Key);
//			}
//		}

//		foreach (var key in nullEntities)
//		{
//			vfxDictionary.Remove(key);
//		}
//	}
}

