using Client;
using Leopotam.Ecs;

public class EndOfDaySystem : IEcsRunSystem
{
    private GameContext gameContext;
    private SceneConfiguration sceneConfiguration;
    private EcsFilter<EndOfDay> filter;
    public void Run()
    {
        if (!filter.IsEmpty())
        {
            PointsSystem.ChangePoints(sceneConfiguration.hungerEndOfDayPoints);
        }
    }
}