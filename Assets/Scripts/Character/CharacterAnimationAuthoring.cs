using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterAnimationAuthoring : MonoBehaviour
{
    public class Baker : Baker<CharacterAnimationAuthoring>
    {
        public override void Bake(CharacterAnimationAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            CharacterAnimation characterAnimation = new CharacterAnimation();

            // Set clip indexes
            characterAnimation.IdleClip = 0;
            characterAnimation.RunClip = 1;
            characterAnimation.SprintClip = 2;
            characterAnimation.InAirClip = 3;
            characterAnimation.LedgeGrabMoveClip = 4;
            characterAnimation.LedgeStandUpClip = 5;
            characterAnimation.WallRunLeftClip = 6;
            characterAnimation.WallRunRightClip = 7;
            characterAnimation.CrouchIdleClip = 8;
            characterAnimation.CrouchMoveClip = 9;
            characterAnimation.ClimbingMoveClip = 10;
            characterAnimation.SwimmingIdleClip = 11;
            characterAnimation.SwimmingMoveClip = 12;
            characterAnimation.DashClip = 13;
            characterAnimation.RopeHangClip = 14;
            characterAnimation.SlidingClip = 15;

            AddComponent(entity, characterAnimation);
        }
    }
}