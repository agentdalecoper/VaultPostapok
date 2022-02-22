using Leopotam.Ecs;

public class EndOfDaySystem : IEcsRunSystem
{
    private GameContext gameContext;
    public void Run()
    {
        // todo also refactor these checks to service
        if (gameContext.dayCards.Count == 0 && (!gameContext.currentCard.HasValue || !gameContext.currentCard.Value.IsAlive()))
        {
            EndOfDayUI.Instance.gameObject.SetActive(true);
            EndOfDayUI.Instance.SetData(gameContext.dayNumber);
        }
    }
}