using Client;
using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;

sealed class EcsStartupManager : MonoBehaviour
{
    EcsWorld _world;
    EcsSystems _systems;
    public SceneConfiguration sceneConfiguration;

    void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
        _systems
            .Add(new PointsSystem())
            .Add(new SwipeSystem())
            .Inject(sceneConfiguration)
            .Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
        }
    }
}

internal class SwipeSystem : IEcsInitSystem
{
    public void Init()
    {
    }

    public static void SwipedRight<TData, TContext>(UISwipeableCard<TData, TContext> card) where TContext : class
    {
        float randomForEvent = Random.value;
        if (randomForEvent < 0.3f)
        {
            PointsSystem
                .ChangePoints(new PointsComponent()
                {
                    money = Random.Range(1, 2),
                    food = Random.Range(1, 2)
                });
        }
        else if (randomForEvent < 0.6f)
        {
            PointsSystem
                .ChangePoints(new PointsComponent() {money = Random.Range(1, 2)});
        }
        else
        {
            PointsSystem
                .ChangePoints(new PointsComponent() {hull = Random.Range(-1, -4)});
        }
    }

    public static void SwipedLeft<TData, TContext>(UISwipeableCard<TData, TContext> card) where TContext : class
    {
        float randomForEvent = Random.value;
        if (randomForEvent < 0.5f)
        {
            PointsSystem
                .ChangePoints(new PointsComponent()
                {
                    hull = Random.Range(1, 2),
                });
        }
        else
        {
            PointsSystem
                .ChangePoints(new PointsComponent() {hull = Random.Range(2, 5)});
        }
    }
}