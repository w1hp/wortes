using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Player : IComponentData
{
    public Entity ControlledCharacter;
    public Entity ControlledCamera;
}

[Serializable]
public struct PlayerInputs : IComponentData
{
    public float2 Move;
    public float2 Look;
    public float CameraZoom;
    
    public bool SprintHeld;
    public bool RollHeld;
    public bool JumpHeld;
    public bool ShootOrBuildHeld;


	public FixedInputEvent JumpPressed;
    public FixedInputEvent DashPressed;
    public FixedInputEvent CrouchPressed;
    public FixedInputEvent RopePressed;
    public FixedInputEvent ClimbPressed;
    public FixedInputEvent FlyNoCollisionsPressed;
    public FixedInputEvent SwitchModePressed;
	public FixedInputEvent ShootOrBuildPressed;

    public byte ChooseSlot;
}
