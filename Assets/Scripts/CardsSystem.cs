using Leopotam.Ecs;
using UnityEngine;

internal class CardsSystem : IEcsRunSystem
{
    private EcsWorld ecsWorld;
    private GameContext gameContext;

    public void Run()
    {
        if (!gameContext.currentCard.HasValue || !gameContext.currentCard.Value.IsAlive())
        {
            if (gameContext.dayCards.Count == 0)
            {
                return;
            }
            
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
            else
            {
                Debug.LogError("This card is not known type " + cardEntity);
            }
        }
    }

    private static void SetActiveTradeUi()
    {
        CardUI.Instance.gameObject.SetActive(false);
        TradeUI.Instance.gameObject.SetActive(true);
    }
    
    private static void SetActiveCardUi()
    {
        CardUI.Instance.gameObject.SetActive(true);
        TradeUI.Instance.gameObject.SetActive(false);
    }

}