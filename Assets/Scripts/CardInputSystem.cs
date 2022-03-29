using Client;
using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;

internal class CardInputSystem : IEcsRunSystem
{
    private EcsFilter<SwipeDirection> swipeFilter;
    private PointsSystem pointsSystem;
    private GameContext gameContext;
    private EcsWorld ecsWorld;
    private SceneConfiguration sceneConfiguration;

    public void Run()
    {
        foreach (int i in swipeFilter)
        {
            ref SwipeDirection swipeDirection = ref swipeFilter.Get1(i);

            if (!gameContext.currentCard.HasValue)
            {
                Debug.LogError("Swiped but current card is null");
                return;
            }

            EcsEntity currentCard = gameContext.currentCard.Value;
            ref CardInfo cardInfo = ref currentCard.Get<CardInfo>();

            if (cardInfo.nextCards.Count == 0)
            {
            }
            else if (swipeDirection == SwipeDirection.Left)
            {
                if (!cardInfo.nextCards[0].Has<CardStub>())
                {
                    gameContext.currentCard = cardInfo.nextCards[0];
                    gameContext.currentCard.Value.Get<Render>();
                    Debug.Log("next card is left card " + gameContext.currentCard);
                }
            }
            else
            {
                if (!cardInfo.nextCards[1].Has<CardStub>())
                {
                    gameContext.currentCard = cardInfo.nextCards[1];
                    gameContext.currentCard.Value.Get<Render>();
                    Debug.Log("next card is right card " + gameContext.currentCard);
                }
            }

            Debug.Log("Destroying card " + currentCard);
            currentCard.Destroy();
        }
        if (!gameContext.currentCard.HasValue || !gameContext.currentCard.Value.IsAlive())
        {
            EcsEntity cardEntity = gameContext.dayCards[0];
            gameContext.dayCards.Remove(cardEntity);

            Debug.Log("Next card is " + cardEntity + " " + cardEntity.Get<CardInfo>().text);
            
            if (!cardEntity.IsAlive())
            {
                Debug.LogError("Card entity " + cardEntity + " is not alive");
            }
            
            gameContext.currentCard = cardEntity;
            ref CardInfo cardInfo = ref cardEntity.Get<CardInfo>();
            gameContext.currentCard.Value.Get<Render>();
        }
        
    }
    
      
    // if (currentCard.Has<SkillsLeftRight>())
    // {
    //     SkillsLeftRight skillsLeftRight = currentCard.Get<SkillsLeftRight>();
    //     SkillsComponent skillsComponent = skillsLeftRight.GetSwipe(swipeDirection);
    //     SkillsSystem.Instance.CreateSkillsUpdate(skillsComponent);
    // }
    // else if (currentCard.Has<PointsLeftRight>() && !currentCard.Has<SkillsCheck>())
    // {
    //     PointsLeftRight pointsLeftRight = currentCard.Get<PointsLeftRight>();
    //     PointsComponent pointsComponent = pointsLeftRight.GetSwipe(swipeDirection);
    //     PointsSystem.ChangePoints(pointsComponent);
    // }
    // else if (currentCard.Has<PointsLeftRight>() && currentCard.Has<SkillsCheck>())
    // {
    // }
    // else if (currentCard.Has<Trade>())
    // {
    // }
    // else if(currentCard.Get<CardInfo>().cardType == CardType.EndOfDay)
    // {
    //     PointsSystem.ChangePoints(sceneConfiguration.hungerEndOfDayPoints);
    //     EcsEntity entity = ecsWorld.NewEntity();
    //     entity.Get<EndOfDay>();
    // }
    // else
    // {
    //     Debug.LogError("Swiped but current card is not a swipe card or skill choose card " + currentCard);
    //     return;
    // }
}

internal struct Render
{
}

internal struct EndOfDay
{
}