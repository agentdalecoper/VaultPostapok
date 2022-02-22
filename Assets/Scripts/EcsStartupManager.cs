using System;
using System.Threading.Tasks;
using Client;
using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;

public struct Trade
{
    public SkillsComponent[] skillsComponents;
    public int[] skillComponentsCosts;
    public PointsComponent[] pointsComponents;
    public int[] pointsComponentsCosts;

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
         * SceneConfiguration
         *
         * CardInfo [string text; sprite spr;]
         * PointsLeftRight [PointsComponent left; PointsComponent right;]
         * SkillsLeftRight [SkillsComponent left; SkillsComponent right;]
         * SkillsCheck [SkillsComponent skillsToCheck;]
         * Trade[SkillsComponent upgrades[3], int hullCost, int foodCost]
         * 
         * SwipeCardEntity [CardInfo, PointsLeftRight]
         * LevelUpCardEntity[CardInfo, SkillsLeftRight]
         * SkillRollCardEntity[CardInfo, SkillsCheck, PointsLeftRight]
         * TradeCardEntity[CardInfo, Trade] нужно ли через card представлять
         *
         * 1) CardsCreateSystem (creates cards right now without a day etc)
         * 1.1) we have cards[EcsEntityCard, EcsEntityCard ....]
         *
         * 2) CardSystem - cards.First() ++ currentCard
         * 2.1) onCardAppeared?.Invoke()
         *
         * 3) CardUI - show UI with SwipeCard, LevelUp, SkillRoll or TradeCard of EcsCardEntity
         * 
         * 3.1) UI for SwipeCard or LevelUp: await swipe => EcsEntity[SwipeDirection]
         * 3.1.1) SwipeSystem: await EcsEntity[SwipeDirection] => calculateResources(); onSwipeResult?.Invoke();
         * 3.1.1) UI for SwipeCard or LevelUp: await onSwipeResult => render results
         * 
         * 3.2) UI for SkillRoll: await roll clicked => create EcsEntity[RollClicked]
         * 3.2.1) SkillSystem: await EcsEntity[RollClicked] => doARoll(); calculateWinOrLoss(); caclulateResources();
         *                                                          onRollSkillResult?.Invoke(actualRoll);
         * 3.2.2) UI for SkillRoll: await onRollSkillResult => render results
         *
         * 3.3) Trade: show different screenUI => if bought points => EcsEntity[Trade, PointsComponent]
         *                                        if bought resource => EcsEntity[Trade, SkillsComponent]
         * 3.3.1) TradeSystem - await EcsEntity[Trade] => and resoruce and remove cost
         * 3.3.2) Trade UI => rerender UI
         *
         * По поводу уишки блин - реально идет выяснение что лучше:
         *      - контроль от юзера через прямой вызов сервиса или через создание командных EcsEntity
         *      - передача данных UI через event или через EcsFilter 
         */

        GameContext gameContext = new GameContext();
        PointsSystem pointsSystem = new PointsSystem();
        _systems
            .Add(new CardsInitSystem())
            .Add(new CardInputSystem())
            .Add(new CardsSystem())
            .Add(new DiceSystem())
            .Add(pointsSystem)
            .Add(new SwipeSystem())
            .Add(new SkillsSystem())
            .Add(new SkillsRollSystem())
            .Add(new TradeSystem())
            .Inject(sceneConfiguration)
            .Inject(gameContext)
            .Inject(pointsSystem)
            .OneFrame<SwipeDirection>()
            .OneFrame<DiceClick>()
            .OneFrame<DiceRoll>()
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

internal class TradeSystem : IEcsInitSystem, IEcsRunSystem
{
    public void Init()
    {
        TradeUI.Instance.onBoughtSkills += (skillsComponent, cost) =>
        {
            SkillsSystem.Instance.CreateSkillsUpdate(skillsComponent);
            PointsSystem.ChangePoints(new PointsComponent {money = -cost});
        };

        TradeUI.Instance.onBoughtPoints += (points, cost) =>
        {
            PointsSystem.ChangePoints(points);
            PointsSystem.ChangePoints(new PointsComponent {money = -cost});
        };
    }
    
    public void Run()
    {
        
    }

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
}

public struct CardInfo
{
    public string text;
    public Sprite sprite;
}

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

public struct SkillsCheck
{
    public SkillsComponent skillsToCheck;
}