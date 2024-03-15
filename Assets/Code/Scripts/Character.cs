using Unity.Entities;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float speed = 2f;
}

public struct CharacterData : IComponentData
{
	public float speed;
}

public class  CharacterBake : Baker<Character>
{
	public override void Bake(Character authoring)
	{
		var entity = GetEntity(TransformUsageFlags.Dynamic);

		AddComponent(entity, new CharacterData
		{
			speed = authoring.speed
		});

		AddComponent(entity, new InputsData
		{

		});

	}
}