using Unity.Entities;
using Unity.CharacterController;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public struct FlyingNoCollisionsState : ICharacterState
{
    public void OnStateEnter(CharacterState previousState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        ref PhysicsCollider characterCollider = ref aspect.KinematicCharacterAspect.PhysicsCollider.ValueRW;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        
        aspect.SetCapsuleGeometry(character.StandingGeometry.ToCapsuleGeometry());
        
        KinematicCharacterUtilities.SetCollisionDetectionActive(false, ref characterProperties, ref characterCollider);
        characterBody.IsGrounded = false;
    }

    public void OnStateExit(CharacterState nextState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        ref PhysicsCollider characterCollider = ref aspect.KinematicCharacterAspect.PhysicsCollider.ValueRW;
        
        KinematicCharacterUtilities.SetCollisionDetectionActive(true, ref characterProperties, ref characterCollider);
    }

    public void OnStatePhysicsUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        float deltaTime = baseContext.Time.DeltaTime;
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        ref CharacterControl characterControl = ref aspect.CharacterControl.ValueRW;
        ref float3 characterPosition = ref aspect.KinematicCharacterAspect.LocalTransform.ValueRW.Position;
        
        aspect.KinematicCharacterAspect.Update_Initialize(in aspect, ref context, ref baseContext, ref characterBody, deltaTime);
        
        // Movement
        float3 targetVelocity = characterControl.MoveVector * character.FlyingMaxSpeed;
        CharacterControlUtilities.InterpolateVelocityTowardsTarget(ref characterBody.RelativeVelocity, targetVelocity, deltaTime, character.FlyingMovementSharpness);
        characterPosition += characterBody.RelativeVelocity * deltaTime;

        aspect.DetectGlobalTransitions(ref context, ref baseContext);
    }

    public void OnStateVariableUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref quaternion characterRotation = ref aspect.KinematicCharacterAspect.LocalTransform.ValueRW.Rotation;
        
        characterRotation = quaternion.identity;
    }

    public void GetCameraParameters(in CharacterComponent character, out Entity cameraTarget, out bool calculateUpFromGravity)
    {
        cameraTarget = character.DefaultCameraTargetEntity;
        calculateUpFromGravity = false;
    }

    public void GetMoveVectorFromPlayerInput(in PlayerInputs inputs, quaternion cameraRotation, out float3 moveVector)
    {
        CharacterAspect.GetCommonMoveVectorFromPlayerInput(in inputs, cameraRotation, out moveVector);
        float verticalInput = (inputs.JumpHeld ? 1f : 0f) + (inputs.RollHeld ? -1f : 0f);
        moveVector = MathUtilities.ClampToMaxLength(moveVector + (math.mul(cameraRotation, math.up()) * verticalInput), 1f);
    }
}