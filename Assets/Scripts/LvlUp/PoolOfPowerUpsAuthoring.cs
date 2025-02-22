//using UnityEngine;
//using Unity.Entities;
//using System.Collections.Generic;

//public class PoolOfPowerUpsAuthoring : MonoBehaviour
//{
//	public List<PowerUpSO> powerUpsList;

//	class PoolOfPowerUpsBaker : Baker<PoolOfPowerUpsAuthoring>
//	{
//		public override void Bake(PoolOfPowerUpsAuthoring authoring)
//		{
//			var entity = GetEntity(TransformUsageFlags.None);
//			AddComponent(entity, new PoolOfPowerUps());
//		}
//	}
//}

//public struct PoolOfPowerUps : IComponentData
//{
//    //TASK: Why did I create this?
//}
