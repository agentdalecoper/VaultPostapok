using Leopotam.Ecs;
using UnityEngine;

internal class CardsSystem : IEcsRunSystem
{
    private EcsWorld ecsWorld;

    // ну и что вот у нас получается куча карт cards[] - какая из них текущая?
    // так ну что лучше по одному ивенту на каждый тип карты? Криво. Или отсюда короче напрямую вызывать
    // лол походу пришло время писать криво и костыли ибо чето опять же хз как сделать этот паттерн - потом увидим.
    // либо ивент делать не просто из функции а передаввать уишке весь стейт за счет функции хммм но короче потом
    private GameContext gameContext;

    public void Run()
    {
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