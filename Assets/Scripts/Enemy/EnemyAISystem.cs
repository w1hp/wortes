using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public partial struct EnemyAISystem : ISystem
{
    private EntityManager entityManager;
    private Entity characterEntity;

    private void OnCreate(ref SystemState state)
    {
        entityManager = state.EntityManager;
    }

    private void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<CharacterAnimation>(out characterEntity))
        {
            // Jeśli jednostka singletona nie istnieje, zwróć
            return;
        }

        float groundLevel = 1.0f; // Zdefiniuj poziom ziemi, np. 0

        foreach (var (enemyComponent, transformComponent) in SystemAPI.Query<EnemyComponent, RefRW<LocalTransform>>())
        {
            // Pobierz pozycję postaci, ustawiając jej Y na poziom ziemi
            float3 characterPosition = entityManager.GetComponentData<LocalTransform>(characterEntity).Position;
            characterPosition.y = groundLevel;

            // Pobierz pozycję wroga, ustawiając jego Y na poziom ziemi
            float3 enemyPosition = transformComponent.ValueRO.Position;
            enemyPosition.y = groundLevel;

            // Oblicz kierunek w płaszczyźnie XZ
            float3 direction = characterPosition - enemyPosition;
            float angle = math.atan2(direction.z, direction.x);

            // Ustaw obrót wroga w płaszczyźnie XZ
            transformComponent.ValueRW.Rotation = quaternion.Euler(new float3(0, angle, 0));

            // Zaktualizuj pozycję wroga, poruszając się tylko w płaszczyźnie XZ
            float3 normalizedDirection = math.normalize(direction);
            float3 newPosition = enemyPosition + normalizedDirection * SystemAPI.Time.DeltaTime;
            newPosition.y = groundLevel; // Upewnij się, że nowa pozycja ma odpowiedni poziom Y

            transformComponent.ValueRW.Position = newPosition;
        }
    }
}
