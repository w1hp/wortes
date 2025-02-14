using Unity.Burst;
using Unity.Entities;

partial struct HealthUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach(var health in SystemAPI.Query<Health>().WithAll<BossTag>())
		{
            if (BossHealthPresenter.Singleton == null) return;

			var bossHealthPresenter = BossHealthPresenter.Singleton;
			bossHealthPresenter.UpdateView((int)health.CurrentHealth, (int)health.MaxHealth);
		}
        foreach (var health in SystemAPI.Query<Health>().WithAll<PlayerTag>())
        {
            if (PlayerHealthPresenter.Singleton == null) return;

            var playerHealthPresenter = PlayerHealthPresenter.Singleton;
			playerHealthPresenter.UpdateView((int)health.CurrentHealth, (int)health.MaxHealth);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
