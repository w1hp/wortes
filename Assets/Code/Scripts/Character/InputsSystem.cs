using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public partial class InputsSystem : SystemBase
{
	private Controls inputs = null;
	protected override void OnCreate()
	{
		if (!SystemAPI.TryGetSingleton<InputsData>(out InputsData input))
		{
			EntityManager.CreateEntity(typeof(InputsData));
		}

		inputs = new Controls();
		inputs.Enable();
	}
	protected override void OnUpdate()
	{
		var move = inputs.Character.Move.ReadValue<Vector2>();
		var look = inputs.Character.Look.ReadValue<Vector2>();
		var action = inputs.Character.Action.ReadValue<float>() == 1 ? true : false;
		var switchMode = inputs.Character.SwitchMode.triggered;
		var choose1 = inputs.Character.Choose1.triggered;
		var choose2 = inputs.Character.Choose2.triggered;
		var choose3 = inputs.Character.Choose3.triggered;
		var choose4 = inputs.Character.Choose4.triggered;

		SystemAPI.SetSingleton(new InputsData
		{
			move = move,
			look = look,
			action = action,
			switchMode = switchMode,
			choose1 = choose1,
			choose2 = choose2,
			choose3 = choose3,
			choose4 = choose4
		});
	}
}

public struct InputsData : IComponentData
{
	public float2 move;
	public float2 look;
	public bool action;
	public bool switchMode;
	public bool choose1;
	public bool choose2;
	public bool choose3;
	public bool choose4;
}