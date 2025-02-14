//using Unity.Burst;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Physics.Stateful;
//using Unity.Rendering;
//using Unity.Physics;
//using Unity.Transforms;


//partial struct TowerCollisionDetectionSystem : ISystem
//{

//	[BurstCompile]
//	public void OnCreate(ref SystemState state)
//	{
//		state.RequireForUpdate<TowerCollision>();
//	}

//	[BurstCompile]
//	public void OnUpdate(ref SystemState state)
//	{
//		var nonTriggerQuery = SystemAPI.QueryBuilder().WithNone<StatefulTriggerEvent>().Build();

//		var nonTriggerMask = nonTriggerQuery.GetEntityQueryMask();
//		foreach (var (triggerEventBuffer, towerCollision, entity) in
//			SystemAPI.Query<
//				DynamicBuffer<StatefulTriggerEvent>,
//				RefRW<TowerCollision>>()
//				.WithEntityAccess())
//		{
//			var highlighterChildren = SystemAPI.GetBuffer<Child>(entity);
//			var graphicEntity = highlighterChildren[0].Value;
//			var baseColor = SystemAPI.GetComponentRW<URPMaterialPropertyBaseColor>(graphicEntity);

//			for (int i = 0; i < triggerEventBuffer.Length; i++)
//			{
//				var triggerEvent = triggerEventBuffer[i];
//				var otherEntity = triggerEvent.GetOtherEntity(entity);

//				// exclude other triggers and processed events
//				if (triggerEvent.State == StatefulEventState.Stay ||
//						!nonTriggerMask.MatchesIgnoreFilter(otherEntity))
//				{
//					continue;
//				}
//				if (triggerEvent.State == StatefulEventState.Enter)
//				{
//					towerCollision.ValueRW.CanBuild = false;
//					//RED
//					baseColor.ValueRW.Value = new float4(1, 0, 0, 0.5f);
//				}
//				else if (triggerEvent.State == StatefulEventState.Exit)
//				{
//					towerCollision.ValueRW.CanBuild = true;
//					//GREEN
//					baseColor.ValueRW.Value = new float4(0, 1, 0, 0.5f);
//				}
//			}
//		}
//	}

//	struct CollisionEventImpulseJob : ICollisionEventsJob
//	{
//		public void Execute(CollisionEvent collisionEvent)
//		{ }
//	}
//}