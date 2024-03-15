using UnityEngine;
using Unity.Entities;

public partial class InputsSystem : SystemBase
{
	private Controls inputs = null;
	protected override void OnCreate()
	{
		inputs = new Controls();
		inputs.Enable();
	}
	protected override void OnUpdate()
	{
		foreach (RefRW<InputsData> data in SystemAPI.Query<RefRW<InputsData>>())
		{
			data.ValueRW.move = inputs.Character.Move.ReadValue<Vector2>();
		}
	}
}
