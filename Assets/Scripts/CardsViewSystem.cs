using Leopotam.Ecs;
using UnityEngine;

internal class CardsViewSystem : IEcsRunSystem
{
    private EcsWorld ecsWorld;
    private GameContext gameContext;

    // вот эту систему можно разбить - на ту которая логически решает так нету карты и нужна следуюшая
    // и та, которая выставляет вьюху
    public void Run()
    {
        if (gameContext.dayCards.Count == 0)
        {
            return;
        }
        
        if (!gameContext.currentCard.HasValue || !gameContext.currentCard.Value.IsAlive())
        {
            EcsEntity cardEntity = gameContext.dayCards[0];
            gameContext.dayCards.Remove(cardEntity);

            if (!cardEntity.IsAlive())
            {
                Debug.LogError("Card entity " + cardEntity + " is not alive");
            }

            gameContext.currentCard = cardEntity;
            ref CardInfo cardInfo = ref cardEntity.Get<CardInfo>();

            //todo unify card check logic
            if (cardEntity.Has<PointsLeftRight>() && !cardEntity.Has<SkillsCheck>())
            {
                SetActiveCardUi();
                CardUI.Instance.ShowCardData(cardEntity, cardInfo, cardEntity.Get<PointsLeftRight>());
            }
            else if (cardEntity.Has<SkillsLeftRight>())
            {
                SetActiveCardUi();
                CardUI.Instance.ShowCardData(cardEntity, cardInfo, cardEntity.Get<SkillsLeftRight>());
            }
            else if (cardEntity.Has<PointsLeftRight>() && cardEntity.Has<SkillsCheck>())
            {
                SetActiveCardUi();
                CardUI.Instance.ShowCardData(cardEntity, cardInfo,
                    cardEntity.Get<PointsLeftRight>(), cardEntity.Get<SkillsCheck>());
            }
            else if (cardEntity.Has<Trade>())
            {
                SetActiveTradeUi();
                TradeUI.Instance.ShowCard(cardEntity.Get<Trade>());
            }
            else if (cardEntity.Get<CardInfo>().cardType == CardType.EndOfDay)
            {
                EndOfDayUI.Instance.SetData(gameContext.dayNumber);
                SetActiveEndOfDayUi();
            }
            else
            {
                Debug.LogError("This card is not known type " + cardEntity);
            }
        }
    }

    private static void SetActiveEndOfDayUi()
    {
        CardUI.Instance.gameObject.SetActive(false);
        TradeUI.Instance.gameObject.SetActive(false);
        EndOfDayUI.Instance.gameObject.SetActive(true);
    }

    private static void SetActiveTradeUi()
    {
        CardUI.Instance.gameObject.SetActive(false);
        TradeUI.Instance.gameObject.SetActive(true);
        EndOfDayUI.Instance.gameObject.SetActive(false);
    }
    
    private static void SetActiveCardUi()
    {
        CardUI.Instance.gameObject.SetActive(true);
        TradeUI.Instance.gameObject.SetActive(false);
        EndOfDayUI.Instance.gameObject.SetActive(false);
    }

}