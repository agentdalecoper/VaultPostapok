using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;

internal class CardsViewSystem : IEcsRunSystem
{
    private EcsWorld ecsWorld;
    private GameContext gameContext;

    private EcsFilter<CardInfo, Render> renderFilter;

    // вот эту систему можно разбить - на ту которая логически решает так нету карты и нужна следуюшая
    // и та, которая выставляет вьюху
    public void Run()
    {
        if (!renderFilter.IsEmpty())
        {
            EcsEntity cardEntity = renderFilter.GetEntity(0);
            ref CardInfo cardInfo = ref cardEntity.Get<CardInfo>();

            Debug.Log("Show card " + cardEntity + " " + cardInfo.text);

            if (cardInfo.cardObject.tradeTest.IsSet)
            {
                SetActiveTradeUi();
                TradeUI.Instance.ShowCard(cardInfo.cardObject.tradeTest.Value);
            }
            else if (cardInfo.cardObject.skillsCheck.IsSet)
            {
                SetActiveCardUi();
                CardUI.Instance.ShowCardData(cardEntity, cardInfo, cardInfo.cardObject.skillsCheck.Value);
            }
            else
            {
                SetActiveCardUi();
                CardUI.Instance.ShowCardData(cardEntity, cardInfo);
            }

            
            // //todo unify card check logic
            // if (cardEntity.Has<PointsLeftRight>() && !cardEntity.Has<SkillsCheck>())
            // {
            //     SetActiveCardUi();
            //     CardUI.Instance.ShowCardData(cardEntity, cardInfo, cardEntity.Get<PointsLeftRight>());
            // }
            // else if (cardEntity.Has<SkillsLeftRight>())
            // {
            //     SetActiveCardUi();
            //     CardUI.Instance.ShowCardData(cardEntity, cardInfo, cardEntity.Get<SkillsLeftRight>());
            // }
            // else if (cardEntity.Has<PointsLeftRight>() && cardEntity.Has<SkillsCheck>())
            // {
            //     SetActiveCardUi();
            //     CardUI.Instance.ShowCardData(cardEntity, cardInfo,
            //         cardEntity.Get<PointsLeftRight>(), cardEntity.Get<SkillsCheck>());
            // }
            // else if (cardEntity.Has<Trade>())
            // {
            //     SetActiveTradeUi();
            //     TradeUI.Instance.ShowCard(cardEntity.Get<Trade>());
            // }
            // else if (cardEntity.Get<CardInfo>().cardType == CardType.EndOfDay)
            // {
            //     EndOfDayUI.Instance.SetData(gameContext.dayNumber);
            //     SetActiveEndOfDayUi();
            // }
            // else
            // {
            //     Debug.LogError("This card is not known type " + cardEntity);
            // }
        }
        
        if (gameContext.dayCards.Count == 0)
        {
            return;
        }

        if (gameContext.endOfGame.HasValue)
        {
            EndOfGameUI.Instance.SetData(gameContext.endOfGame.Value);
            SetActiveEndOfGame();
            return;
        }

        
    }

    private static void SetActiveEndOfGame()
    {
        CardUI.Instance.gameObject.SetActive(false);
        TradeUI.Instance.gameObject.SetActive(false);
        EndOfDayUI.Instance.gameObject.SetActive(false);
        EndOfGameUI.Instance.gameObject.SetActive(true);
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