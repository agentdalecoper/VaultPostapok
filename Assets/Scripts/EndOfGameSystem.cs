using Client;
using Leopotam.Ecs;

internal class EndOfGameSystem : IEcsRunSystem
{
    private GameContext gameContext;
    private SceneConfiguration sceneConfiguration;
    private EcsWorld ecsWorld;

    private EcsFilter<PointsComponent> pointsFilter;

    public void Run()
    {
        PointsComponent points = pointsFilter.Get1(0);
        if (points.food <= 0 || points.hull <= 0)
        {
            var endGameEntity = ecsWorld.NewEntity();
            ref var endOfGame = ref endGameEntity.Get<EndOfGame>();
            endOfGame.win = false;
            gameContext.endOfGame = endOfGame;
            return;
        }

        if (gameContext.dayCards.Count == 0 && gameContext.dayNumber == sceneConfiguration.days.Length - 1)
        {
            var endGameEntity = ecsWorld.NewEntity();
            ref var endOfGame = ref endGameEntity.Get<EndOfGame>();
            endOfGame.win = true;
            gameContext.endOfGame = endOfGame;
        }
    }
}