using Unity.Entities;
using UnityEngine;

class AudioSorurceAuthoring : MonoBehaviour
{
	public AudioType AudioType;
	class Baker : Baker<AudioSorurceAuthoring>
	{
		public override void Bake(AudioSorurceAuthoring authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

			AddComponent(entity, new AudioSorurceComponent
			{
				Type = authoring.AudioType
			});
		}
	}
}

public struct AudioSorurceComponent : IComponentData
{
	//public int SoundType;
	public AudioType Type;
}