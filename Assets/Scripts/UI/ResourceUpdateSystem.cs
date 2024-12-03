using Unity.Burst;
using Unity.Entities;

partial struct ResourceUpdateSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CharacterResources>();
	}

	public void OnUpdate(ref SystemState state)
	{
		if (ResourceController.Singleton == null)
			return;
		var recourseController = ResourceController.Singleton;

		CharacterResources characterResources;
		if (!SystemAPI.TryGetSingleton<CharacterResources>(out characterResources))
			return;

		recourseController.UpdateResourceText(characterResources.Wood, characterResources.Fire, characterResources.Water, characterResources.Earth, characterResources.Metal);

	}
}
