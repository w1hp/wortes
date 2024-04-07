using Unity.Entities;
using UnityEngine;

public class CharacterModelAuthoring : MonoBehaviour
{
	public GameObject gun;
	public GameObject highlighter;
	class Baker : Baker<CharacterModelAuthoring>
	{
		public override void Bake(CharacterModelAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new CharacterModel
			{
				gun = GetEntity(authoring.gun, TransformUsageFlags.Dynamic),
				highlighter = GetEntity(authoring.highlighter, TransformUsageFlags.Dynamic),
			});
		}
	}
}

public struct CharacterModel : IComponentData
{
	public Entity gun;
	public Entity highlighter;
}