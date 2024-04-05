using Unity.Entities;
using UnityEngine;

public class CharacterAuthoring : MonoBehaviour
{
	public float moveSpeed = 2f;
	public float dashSpeed = 5f;
	public float mouseSensitivity = 1f;
	public float shootCooldown = 0.5f;

	class Baker : Baker<CharacterAuthoring>
	{
		public override void Bake(CharacterAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);

			AddComponent(entity, new Character
			{
				moveSpeed = authoring.moveSpeed,
				dashSpeed = authoring.dashSpeed,
				mouseSensitivity = authoring.mouseSensitivity,
				shootCooldown = authoring.shootCooldown,
			});
		}
	}
}

public struct Character : IComponentData
{
	public float moveSpeed;
	public float dashSpeed;
	public float mouseSensitivity;
	public float shootCooldown;
}