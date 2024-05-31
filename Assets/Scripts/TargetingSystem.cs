using System.Collections.Generic;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Transforms;

partial struct TargetingSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{

	}

	public void OnUpdate(ref SystemState state)
	{
		var targetQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform>().WithNone<Target>().Build();
		var kdQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, Target>().Build();

		var targetEntities = targetQuery.ToEntityArray(state.WorldUpdateAllocator);
		var targetTransforms =
				targetQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);

		var tree = new KDTree(targetEntities.Length, Allocator.TempJob, 64);

		// init KD tree
		for (int i = 0; i < targetEntities.Length; i += 1)
		{
			// NOTE - the first parameter is ignored, only the index matters
			tree.AddEntry(i, targetTransforms[i].Position);
		}

		state.Dependency = tree.BuildTree(targetEntities.Length, state.Dependency);

		var queryKdTree = new QueryKDTree
		{
			Tree = tree,
			TargetEntities = targetEntities,
			Scratch = default,
			TargetHandle = SystemAPI.GetComponentTypeHandle<Target>(),
			LocalTransformHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>(true)
		};
		state.Dependency = queryKdTree.ScheduleParallel(kdQuery, state.Dependency);

		state.Dependency.Complete();
		tree.Dispose();
	}

	[BurstCompile]
	public struct QueryKDTree : IJobChunk
	{
		[ReadOnly] public NativeArray<Entity> TargetEntities;
		public PerThreadWorkingMemory Scratch;
		public KDTree Tree;

		public ComponentTypeHandle<Target> TargetHandle;
		[ReadOnly] public ComponentTypeHandle<LocalTransform> LocalTransformHandle;

		public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask,
			in v128 chunkEnabledMask)
		{
			var targets = chunk.GetNativeArray(ref TargetHandle);
			var transforms = chunk.GetNativeArray(ref LocalTransformHandle);

			for (int i = 0; i < chunk.Count; i++)
			{
				if (!Scratch.Neighbours.IsCreated)
				{
					Scratch.Neighbours = new NativePriorityHeap<KDTree.Neighbour>(1, Allocator.Temp);
				}

				Scratch.Neighbours.Clear();
				Tree.GetEntriesInRangeWithHeap(unfilteredChunkIndex, transforms[i].Position, float.MaxValue,
					ref Scratch.Neighbours);
				var nearest = Scratch.Neighbours.Peek().index;
				targets[i] = new Target { Value = TargetEntities[nearest] };
			}
		}
	}

	public struct PerThreadWorkingMemory
	{
		[NativeDisableContainerSafetyRestriction]
		public NativePriorityHeap<KDTree.Neighbour> Neighbours;
	}
}
