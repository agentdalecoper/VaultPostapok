using System;
using Client;
using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;
using Random = UnityEngine.Random;

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
            .Add(new SkillsSystem())
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

internal class SkillsSystem : IEcsInitSystem, IEcsRunSystem
{
    public SkillsComponent playerSkills;
    private SceneConfiguration sceneConfiguration;

    private EcsFilter<SkillsComponent, AddToPlayer> filter;
    private EcsWorld ecsWorld;

    private static SkillsSystem instance;
    public static SkillsSystem Instance => instance;

    public static event Action<SkillsComponent, SkillsComponent> onSkillsChanged;

    public void Init()
    {
        instance = this;
        playerSkills = sceneConfiguration.startSkills;
        onSkillsChanged?.Invoke(playerSkills, playerSkills);
    }

    public void Run()
    {
        foreach (int i in filter)
        {
            ref SkillsComponent skillsComponent = ref filter.Get1(i);
            playerSkills.charisma += skillsComponent.charisma;
            playerSkills.fighting += skillsComponent.fighting;
            playerSkills.mechanical += skillsComponent.mechanical;
            playerSkills.survival += skillsComponent.survival;
            playerSkills.science += skillsComponent.science;

            TextPopUpSpawnerManager.Instance.StartTextPopUpTween(skillsComponent.ToString(),
                Color.green);
            onSkillsChanged?.Invoke(skillsComponent, playerSkills);

            filter.GetEntity(i).Destroy();
        }
    }

    public EcsEntity CreateSkillsUpdate(SkillsComponent skillsComponent)
    {
        EcsEntity entity = ecsWorld.NewEntity();
        entity.Get<AddToPlayer>();
        entity.Replace(skillsComponent);
        return entity;
    }
}

internal class SwipeSystem : IEcsInitSystem
{
    private EcsWorld ecsWorld;


    private static SwipeSystem instance;
    public static SwipeSystem Instance => instance;

    public void Init()
    {
        instance = this;

        UISwipeableViewBasic.ActionSwipedRight += SwipedRight;
        UISwipeableViewBasic.ActionSwipedLeft += SwipedLeft;
    }

    public static void NewCardAppeared<TData, TContext>(UISwipeableCard<TData, TContext> card)
        where TContext : class
    {
        if (Random.value < 0.2f)
        {
            Debug.Log("Dice view should appear");
        }
    }

    public void SwipedRight<TData, TContext>(UISwipeableCard<TData, TContext> card) where TContext : class
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
        else if (randomForEvent < 0.8f)
        {
            SkillsSystem.Instance.CreateSkillsUpdate(new SkillsComponent {fighting = 1});
        }
        else
        {
            PointsSystem
                .ChangePoints(new PointsComponent() {hull = Random.Range(-1, -4)});
        }
    }


    public void SwipedLeft<TData, TContext>(UISwipeableCard<TData, TContext> card) where TContext : class
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