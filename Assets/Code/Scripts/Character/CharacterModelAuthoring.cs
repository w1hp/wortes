using Unity.Entities;
using UnityEngine;

public class CharacterModelAuthoring : MonoBehaviour
{
	class Baker : Baker<CharacterModelAuthoring>
	{
		public override void Bake(CharacterModelAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent(entity, new CharacterModel
			{

			});
		}
	}
}

public struct CharacterModel : IComponentData
{

}