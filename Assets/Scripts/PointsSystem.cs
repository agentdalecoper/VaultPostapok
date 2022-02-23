using System.Drawing;
using Client;
using Leopotam.Ecs;
using Color = UnityEngine.Color;

internal class PointsSystem : IEcsInitSystem, IEcsRunSystem
{
    private static EcsEntity pointsEntity;
    private EcsWorld ecsWorld;
    private SceneConfiguration sceneConfiguration;

    public void Run()
    {
    }

    public void Init()
    {
        pointsEntity = ecsWorld.NewEntity();
        pointsEntity.Replace(sceneConfiguration.startupPoints);
        PointsChanged(sceneConfiguration.startupPoints);
    }

    public static bool EnoughPointsCost(PointsComponent costPoints)
    {
        ref var pointsComponent = ref pointsEntity.Get<PointsComponent>();

        if (pointsComponent.money - costPoints.money >= 0 &&
            pointsComponent.food - costPoints.food >= 0 &&
            pointsComponent.hull - costPoints.hull >= 0)
        {
            return true;
        }

        return false;
    }

    // todo такое ощущение тут надо через апдейт ecs entity это делать 
    public static void ChangePoints(PointsComponent inputChangePoints)
    {
        ref var pointsComponent = ref pointsEntity.Get<PointsComponent>();

        pointsComponent.money += inputChangePoints.money;
        pointsComponent.food += inputChangePoints.food;
        pointsComponent.hull += inputChangePoints.hull;

        if (inputChangePoints.money < 0 || inputChangePoints.food < 0 || inputChangePoints.hull < 0)
        {
            TextPopUpSpawnerManager.Instance.StartTextPopUpTween(inputChangePoints.ToString(), Color.red);
        }
        else
        {
            TextPopUpSpawnerManager.Instance.StartTextPopUpTween(inputChangePoints.ToString(), Color.green);
        }

        PointsChanged(pointsComponent);
    }

    private static void PointsChanged(PointsComponent inputChangePoints)
    {
        HeaderUi.Instance.money.text = inputChangePoints.money.ToString();
        HeaderUi.Instance.foods.text = inputChangePoints.food.ToString();
        HeaderUi.Instance.hull.text = inputChangePoints.hull.ToString();
    }
}