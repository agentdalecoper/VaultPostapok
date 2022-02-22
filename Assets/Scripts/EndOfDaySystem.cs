using Client;
using Leopotam.Ecs;

public class EndOfDaySystem : IEcsRunSystem
{
    private GameContext gameContext;
    private SceneConfiguration sceneConfiguration;
    private EcsFilter<EndOfDay> filter;
    private EcsWorld ecsWorld;

    public void Run()
    {
        if (!filter.IsEmpty())
        {
            gameContext.currentDay.Value.Destroy();
            gameContext.dayNumber++;
        }
    }
}

public struct EndOfGame
{
    public bool win;
}