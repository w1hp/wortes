using Unity.Entities;
using UnityEngine;

class KillStatisticsAuthoring : MonoBehaviour
{
	class Baker : Baker<KillStatisticsAuthoring>
	{
		public override void Bake(KillStatisticsAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent<KillStatistics>(entity);
			AddComponent<CountUpTime>(entity);
		}
	}
}


public struct KillStatistics : IComponentData
{
	public int EnemyCount;
	public int PlayerFragCount;
	public int TowerFragCount;
}