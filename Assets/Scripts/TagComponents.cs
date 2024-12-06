using Unity.Entities;

public struct IsExistTag : IComponentData, IEnableableComponent { }

public struct EnemyTag : IComponentData { }

public struct PlayerTag : IComponentData { }

public struct IsNotPause : IComponentData { }
public struct DamageableTag : IComponentData { }