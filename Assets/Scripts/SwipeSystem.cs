using Client;
using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;

internal class SwipeSystem : IEcsInitSystem
{
    private EcsWorld ecsWorld;


    private static SwipeSystem instance;
    public static SwipeSystem Instance => instance;

    public void Init()
    {
        instance = this;
        CardUI.ActionNewCardAppeared += NewCardAppeared;

        CardUI.ActionSwipedRight += SwipedRight;
        CardUI.ActionSwipedLeft += SwipedLeft;
    }

    public static void NewCardAppeared(EcsEntity cardEntity)
    {
        // if (Random.value < 0.2f)
        // {
        //     Debug.Log("Dice view should appear");
        // }
        // else if(Random.value < 0.5f)
        // {
        //     Debug.Log("Up level should appear");
        // }
    }

    public void SwipedRight(EcsEntity cardEntity)
    {
        EcsEntity entity = ecsWorld.NewEntity();
        entity.Replace(SwipeDirection.Right);
        
        // float randomForEvent = Random.value;
        // if (randomForEvent < 0.3f)
        // {
        //     PointsSystem
        //         .ChangePoints(new PointsComponent()
        //         {
        //             money = Random.Range(1, 2),
        //             food = Random.Range(1, 2)
        //         });
        // }
        // else if (randomForEvent < 0.6f)
        // {
        //     PointsSystem
        //         .ChangePoints(new PointsComponent() {money = Random.Range(1, 2)});
        // }
        // else if (randomForEvent < 0.8f)
        // {
        //     SkillsSystem.Instance.CreateSkillsUpdate(new SkillsComponent {fighting = 1});
        // }
        // else
        // {
        //     PointsSystem
        //         .ChangePoints(new PointsComponent() {hull = Random.Range(-1, -4)});
        // }
    }


    public void SwipedLeft(EcsEntity cardEntity)
    {
        EcsEntity entity = ecsWorld.NewEntity();
        entity.Replace(SwipeDirection.Left);
        
        // float randomForEvent = Random.value;
        // if (randomForEvent < 0.5f)
        // {
        //     PointsSystem
        //         .ChangePoints(new PointsComponent()
        //         {
        //             hull = Random.Range(1, 2),
        //         });
        // }
        // else
        // {
        //     PointsSystem
        //         .ChangePoints(new PointsComponent() {hull = Random.Range(2, 5)});
        // }
    }
}