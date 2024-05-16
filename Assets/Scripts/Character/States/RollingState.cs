using Unity.Entities;
using Unity.CharacterController;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;

public struct RollingState : ICharacterState
{
    public void OnStateEnter(CharacterState previousState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        
        aspect.SetCapsuleGeometry(character.RollingGeometry.ToCapsuleGeometry());
        characterProperties.EvaluateGrounding = false;
        characterBody.IsGrounded = false;

        Utilities.SetEntityHierarchyEnabledParallel(true, character.RollballMeshEntity, context.EndFrameECB, context.ChunkIndex, context.LinkedEntityGroupLookup);
    }

    public void OnStateExit(CharacterState nextState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        ref CharacterComponent character = ref aspect.Character.ValueRW;

        characterProperties.EvaluateGrounding = true;

        Utilities.SetEntityHierarchyEnabledParallel(false, character.RollballMeshEntity, context.EndFrameECB, context.ChunkIndex, context.LinkedEntityGroupLookup);
    }

    public void OnStatePhysicsUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        float deltaTime = baseContext.Time.DeltaTime;
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        ref CharacterControl characterControl = ref aspect.CharacterControl.ValueRW;
        CustomGravity customGravity = aspect.CustomGravity.ValueRO;

        aspect.HandlePhysicsUpdatePhase1(ref context, ref baseContext, true, false);

        CharacterControlUtilities.AccelerateVelocity(ref characterBody.RelativeVelocity, characterControl.MoveVector * character.RollingAcceleration, deltaTime);
        CharacterControlUtilities.AccelerateVelocity(ref characterBody.RelativeVelocity, customGravity.Gravity, deltaTime);
        
        aspect.HandlePhysicsUpdatePhase2(ref context, ref baseContext, false, false, true, false, true);

        DetectTransitions(ref context, ref baseContext, in aspect);
    }

    public void OnStateVariableUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        float deltaTime = baseContext.Time.DeltaTime;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        ref quaternion characterRotation = ref aspect.KinematicCharacterAspect.LocalTransform.ValueRW.Rotation;
        CustomGravity customGravity = aspect.CustomGravity.ValueRO;

        CharacterControlUtilities.SlerpCharacterUpTowardsDirection(ref characterRotation, deltaTime, math.normalizesafe(-customGravity.Gravity), character.UpOrientationAdaptationSharpness);
    }

    public void GetCameraParameters(in CharacterComponent character, out Entity cameraTarget, out bool calculateUpFromGravity)
    {
        cameraTarget = character.DefaultCameraTargetEntity;
        calculateUpFromGravity = true;
    }

    public void GetMoveVectorFromPlayerInput(in PlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        CharacterAspect.GetCommonMoveVectorFromPlayerInput(in inputs, cameraRotation, out moveVector);
    }

    public bool DetectTransitions(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref CharacterControl characterControl = ref aspect.CharacterControl.ValueRW;
        ref CharacterStateMachine stateMachine = ref aspect.StateMachine.ValueRW;
        
        if (!characterControl.RollHeld && aspect.CanStandUp(ref context, ref baseContext))
        {
            stateMachine.TransitionToState(CharacterState.AirMove, ref context, ref baseContext, in aspect);
            return true;
        }

        return aspect.DetectGlobalTransitions(ref context, ref baseContext);
    }
}