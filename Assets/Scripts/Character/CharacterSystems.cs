using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.CharacterController;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[RequireMatchingQueriesForUpdate]
[BurstCompile]
public partial struct CharacterInitializationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<IsNotPause>();
	}

    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingletonRW<EndSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(state.WorldUnmanaged);
        BufferLookup<LinkedEntityGroup> linkedEntitiesLookup = SystemAPI.GetBufferLookup<LinkedEntityGroup>(true);

        foreach (var (character, stateMachine, entity) in SystemAPI.Query<RefRW<CharacterComponent>, RefRW<CharacterStateMachine>>().WithNone<CharacterInitialized>().WithEntityAccess())
        {
            // Make sure the transform system has done a pass on it first
            if (linkedEntitiesLookup.HasBuffer(entity))
            {
                // Disable alternative meshes
                Utilities.SetEntityHierarchyEnabled(false, character.ValueRO.RollballMeshEntity, ecb, linkedEntitiesLookup);
                ecb.AddComponent<CharacterInitialized>(entity);
            }
        }
    }
}

[UpdateInGroup(typeof(KinematicCharacterPhysicsUpdateGroup))]
[BurstCompile]
public partial struct CharacterPhysicsUpdateSystem : ISystem
{
    private EntityQuery _characterQuery;
    private CharacterUpdateContext _context;
    private KinematicCharacterUpdateContext _baseContext;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _characterQuery = KinematicCharacterUtilities.GetBaseCharacterQueryBuilder()
            .WithAll<
                CharacterComponent,
                CharacterControl,
                CharacterStateMachine>()
            .Build(ref state);

        _context = new CharacterUpdateContext();
        _context.OnSystemCreate(ref state);
        _baseContext = new KinematicCharacterUpdateContext();
        _baseContext.OnSystemCreate(ref state);

        state.RequireForUpdate(_characterQuery);
        state.RequireForUpdate<PhysicsWorldSingleton>();
		state.RequireForUpdate<IsNotPause>();
	}

	[BurstCompile]
    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _context.OnSystemUpdate(ref state, SystemAPI.GetSingletonRW<EndSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(state.WorldUnmanaged));
        _baseContext.OnSystemUpdate(ref state, SystemAPI.Time, SystemAPI.GetSingleton<PhysicsWorldSingleton>());
        
        CharacterPhysicsUpdateJob job = new CharacterPhysicsUpdateJob
        {
            Context = _context,
            BaseContext = _baseContext,
        };
        job.ScheduleParallel();
    }

    [BurstCompile]
    [WithAll(typeof(Simulate))]
    public partial struct CharacterPhysicsUpdateJob : IJobEntity, IJobEntityChunkBeginEnd
    {
        public CharacterUpdateContext Context;
        public KinematicCharacterUpdateContext BaseContext;
    
        void Execute([ChunkIndexInQuery] int chunkIndex, CharacterAspect characterAspect)
        {
            Context.SetChunkIndex(chunkIndex);
            characterAspect.PhysicsUpdate(ref Context, ref BaseContext);
        }

        public bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            BaseContext.EnsureCreationOfTmpCollections();
            return true;
        }

        public void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
        { }
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
[BurstCompile]
public partial struct CharacterVariableUpdateSystem : ISystem
{
    private EntityQuery _characterQuery;
    private CharacterUpdateContext _context;
    private KinematicCharacterUpdateContext _baseContext;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _characterQuery = KinematicCharacterUtilities.GetBaseCharacterQueryBuilder()
            .WithAll<
                CharacterComponent,
                CharacterControl>()
            .Build(ref state);

        _context = new CharacterUpdateContext();
        _context.OnSystemCreate(ref state);
        _baseContext = new KinematicCharacterUpdateContext();
        _baseContext.OnSystemCreate(ref state);
        
        state.RequireForUpdate(_characterQuery);
		state.RequireForUpdate<IsNotPause>();
	}

	[BurstCompile]
    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _context.OnSystemUpdate(ref state, SystemAPI.GetSingletonRW<EndSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(state.WorldUnmanaged));
        _baseContext.OnSystemUpdate(ref state, SystemAPI.Time, SystemAPI.GetSingleton<PhysicsWorldSingleton>());
        
        CharacterVariableUpdateJob job = new CharacterVariableUpdateJob
        {
            Context = _context,
            BaseContext = _baseContext,
        };
        job.ScheduleParallel();
    }

    [BurstCompile]
    [WithAll(typeof(Simulate))]
    public partial struct CharacterVariableUpdateJob : IJobEntity, IJobEntityChunkBeginEnd
    {
        public CharacterUpdateContext Context;
        public KinematicCharacterUpdateContext BaseContext;
    
        void Execute([ChunkIndexInQuery] int chunkIndex, CharacterAspect characterAspect)
        {
            Context.SetChunkIndex(chunkIndex);
            characterAspect.VariableUpdate(ref Context, ref BaseContext);
        }

        public bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            BaseContext.EnsureCreationOfTmpCollections();
            return true;
        }

        public void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted)
        { }
    }
}
