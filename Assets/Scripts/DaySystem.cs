using Client;
using Leopotam.Ecs;

internal class DaySystem : IEcsInitSystem, IEcsRunSystem
{
    private GameContext gameContext;
    private EcsWorld ecsWorld;
    private SceneConfiguration sceneConfiguration;

    public void Init()
    {
        SpawnDay();
    }

    public void Run()
    {
        if (!gameContext.currentDay.HasValue || !gameContext.currentDay.Value.IsAlive())
        {
            SpawnDay();
        }
    }

    private void SpawnDay()
    {
        if (gameContext.dayNumber > sceneConfiguration.days.Length - 1)
        {
            return;
        }
        
        EcsEntity dayEntity = ecsWorld.NewEntity();
        dayEntity.Replace(sceneConfiguration.days[gameContext.dayNumber]);
        gameContext.currentDay = dayEntity;

        dayEntity.Get<CreateCard>();
    }
}