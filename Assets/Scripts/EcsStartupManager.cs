using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;
using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;

[Serializable]
public struct Trade
{
    public SkillsComponent[] skillsComponents;
    public int[] skillComponentsCosts;
    public PointsComponent[] pointsComponents;
    public int[] pointsComponentsCosts;
}

[Serializable]
public struct Day
{
    public CardObject[] cardsObjects;
}

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

        /*
         * так - а как вообще должны карты подтягиваться из конфигурации
         * так ну это либо scriptable objects которые наследуются от интерфейса
         * 
         */
        GameContext gameContext = new GameContext();
        PointsSystem pointsSystem = new PointsSystem();
        _systems
            .Add(new DaySystem())
            .Add(new CardsInitSystem())
            .Add(new CardInputSystem())
            .Add(new CardsViewSystem())
            .Add(new DiceSystem())
            .Add(pointsSystem)
            .Add(new SwipeSystem())
            .Add(new SkillsSystem())
            .Add(new SkillsRollSystem())
            .Add(new TradeSystem())
            .Add(new EndOfDaySystem())
            .Inject(sceneConfiguration)
            .Inject(gameContext)
            .Inject(pointsSystem)
            .OneFrame<SwipeDirection>()
            .OneFrame<DiceClick>()
            .OneFrame<DiceRoll>()
            .OneFrame<CreateCard>()
            .OneFrame<EndOfDay>()
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

internal class DaySystem : IEcsInitSystem, IEcsRunSystem
{
    private GameContext gameContext;
    private EcsWorld ecsWorld;
    private SceneConfiguration sceneConfiguration;
    
    public void Init()
    {
        SpawnDay();
    }
    
    public void Run()
    {
        if (!gameContext.currentDay.HasValue || !gameContext.currentDay.Value.IsAlive())
        {
            SpawnDay();
        }
    }

    private void SpawnDay()
    {
        EcsEntity dayEntity = ecsWorld.NewEntity();
        dayEntity.Replace(sceneConfiguration.days[gameContext.dayNumber]);
        gameContext.currentDay = dayEntity;

        dayEntity.Get<CreateCard>();
    }
}

internal struct CreateCard
{
}

public struct DiceRoll
{
    public int roll;
}

public struct DiceClick
{
}


public class GameContext
{
    public EcsEntity? currentCard;
    public List<EcsEntity> dayCards;
    public EcsEntity? currentDay;
    public int dayNumber;
}

public struct CardInfo
{
    public string text;
    public Sprite sprite;
    public CardType cardType;
}

[Serializable]
public struct PointsLeftRight
{
    public PointsComponent left;
    public PointsComponent right;

    public PointsComponent GetSwipe(SwipeDirection swipeDirection)
    {
        if (swipeDirection == SwipeDirection.Left)
        {
            return left;
        }

        return right;
    }
}

[Serializable]
public struct SkillsLeftRight
{
    public SkillsComponent left;
    public SkillsComponent right;

    public SkillsComponent GetSwipe(SwipeDirection swipeDirection)
    {
        if (swipeDirection == SwipeDirection.Left)
        {
            return left;
        }

        return right;
    }
}

[Serializable]
public struct SkillsCheck
{
    public SkillsComponent skillsToCheck;
}