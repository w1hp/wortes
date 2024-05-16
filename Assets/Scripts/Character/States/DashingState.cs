using Unity.Entities;
using Unity.CharacterController;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public struct DashingState : ICharacterState
{
    private float _dashStartTime;
    private float3 _dashDirection;

    public void OnStateEnter(CharacterState previousState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        float elapsedTime = (float)baseContext.Time.ElapsedTime;
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        ref CharacterControl characterControl = ref aspect.CharacterControl.ValueRW;
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref quaternion characterRotation = ref aspect.KinematicCharacterAspect.LocalTransform.ValueRW.Rotation;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        
        aspect.SetCapsuleGeometry(character.StandingGeometry.ToCapsuleGeometry());
        
        _dashStartTime = elapsedTime;
        characterProperties.EvaluateGrounding = false;

        float3 moveVectorOnPlane = math.normalizesafe(MathUtilities.ProjectOnPlane(characterControl.MoveVector, characterBody.GroundingUp)) * math.length(characterControl.MoveVector);
        if (math.lengthsq(moveVectorOnPlane) > 0f)
        {
            _dashDirection = math.normalizesafe(moveVectorOnPlane);
        }
        else
        {
            _dashDirection = MathUtilities.GetForwardFromRotation(characterRotation);
        }
    }

    public void OnStateExit(CharacterState nextState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        
        characterProperties.EvaluateGrounding = true;
        characterBody.RelativeVelocity = float3.zero;
    }

    public void OnStatePhysicsUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        
        aspect.HandlePhysicsUpdatePhase1(ref context, ref baseContext, true, false);

        characterBody.RelativeVelocity = _dashDirection * character.DashSpeed;

        aspect.HandlePhysicsUpdatePhase2(ref context, ref baseContext, false, false, true, false, true);

        DetectTransitions(ref context, ref baseContext, in aspect);
    }

    public void OnStateVariableUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {

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
        float elapsedTime = (float)baseContext.Time.ElapsedTime;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref CharacterStateMachine stateMachine = ref aspect.StateMachine.ValueRW;
        
        if (elapsedTime > _dashStartTime + character.DashDuration)
        {
            if (characterBody.IsGrounded)
            {
                stateMachine.TransitionToState(CharacterState.GroundMove, ref context, ref baseContext, in aspect);
                return true;
            }
            else
            {
                stateMachine.TransitionToState(CharacterState.AirMove, ref context, ref baseContext, in aspect);
                return true;
            }
        }

        return false;
    }
}