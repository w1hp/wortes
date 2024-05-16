using Unity.Entities;
using Unity.CharacterController;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public struct LedgeStandingUpState : ICharacterState
{
    public float3 StandingPoint;
    
    private bool ShouldExitState;

    public void OnStateEnter(CharacterState previousState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        ref CharacterComponent character = ref aspect.Character.ValueRW;
        
        aspect.SetCapsuleGeometry(character.StandingGeometry.ToCapsuleGeometry());
        
        characterBody.RelativeVelocity = default;
        characterBody.IsGrounded = false;

        characterProperties.EvaluateGrounding = false;
        characterProperties.DetectMovementCollisions = false;
        characterProperties.DecollideFromOverlaps = false;

        ShouldExitState = false;
    }

    public void OnStateExit(CharacterState nextState, ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref KinematicCharacterProperties characterProperties = ref aspect.KinematicCharacterAspect.CharacterProperties.ValueRW;
        
        characterProperties.EvaluateGrounding = true;
        characterProperties.DetectMovementCollisions = true;
        characterProperties.DecollideFromOverlaps = true;

        aspect.KinematicCharacterAspect.SetOrUpdateParentBody(ref baseContext, ref characterBody, default, default); 
    }

    public void OnStatePhysicsUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext, in CharacterAspect aspect)
    {
        ref float3 characterPosition = ref aspect.KinematicCharacterAspect.LocalTransform.ValueRW.Position;
        
        aspect.HandlePhysicsUpdatePhase1(ref context, ref baseContext, true, false);

        // TODO: root motion standing up

        characterPosition = StandingPoint;
        ShouldExitState = true;
        
        aspect.HandlePhysicsUpdatePhase2(ref context, ref baseContext, false, false, false, false, true);

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
        ref KinematicCharacterBody characterBody = ref aspect.KinematicCharacterAspect.CharacterBody.ValueRW;
        ref CharacterStateMachine stateMachine = ref aspect.StateMachine.ValueRW;
        
        if (ShouldExitState)
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

        return aspect.DetectGlobalTransitions(ref context, ref baseContext);
    }
}