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

            if (swipeDirection == SwipeDirection.Left)
            {
                gameContext.currentCard = cardInfo.nextCards[0];
            }
            else
            {
                gameContext.currentCard = cardInfo.nextCards[1];
            }

            gameContext.currentCard.Value.Get<Render>();
            
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

            Debug.Log("Destroying card " + currentCard);
            currentCard.Destroy();
        }
    }
}

internal struct Render
{
}

internal struct EndOfDay
{
}