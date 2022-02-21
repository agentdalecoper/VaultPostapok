using System;
using System.Threading.Tasks;
using Client;
using Leopotam.Ecs;
using SwipeableView;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Trade
{
    public SkillsComponent[] skillsComponents;
    public int[] skillComponentsCosts;
    public int hullCost;
    public int foodCost;
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

internal class TradeSystem : IEcsRunSystem
{
    public void Run()
    {
    }
}

internal class SkillsRollSystem : IEcsRunSystem
{
    private EcsFilter<DiceRoll> filter;
    private GameContext gameContext;

    public void Run()
    {
        foreach (int i in filter)
        {
             DiceRoll diceRoll =  filter.Get1(i);

            if (gameContext.currentCard == null)
            {
                Debug.LogError("Dice rolled but card is null");
                return;
            }

            EcsEntity cardEntity = gameContext.currentCard.Value;

            if (!cardEntity.Has<SkillsCheck>())
            {
                Debug.LogError("Dice rolled but card has no skill check");
                return;
            }

            if (!cardEntity.Has<PointsLeftRight>())
            {
                Debug.LogError("Dice rolled - but card has no PointsLeftRight");
                return;
            }

            SkillsCheck skillsCheck =  cardEntity.Get<SkillsCheck>();
            bool success = skillsCheck.skillsToCheck.fighting > diceRoll.roll &&
                           skillsCheck.skillsToCheck.science > diceRoll.roll &&
                           skillsCheck.skillsToCheck.mechanical > diceRoll.roll &&
                           skillsCheck.skillsToCheck.survival > diceRoll.roll &&
                           skillsCheck.skillsToCheck.charisma > diceRoll.roll;


            PointsLeftRight pointsLeftRight = cardEntity.Get<PointsLeftRight>();
            if (!success)
            {
                PointsSystem.ChangePoints(pointsLeftRight.left);
            }
            else
            {
                PointsSystem.ChangePoints(pointsLeftRight.right);
            }
            
            CardUI.Instance.ShowDiceData(diceRoll, success);
        }
    }
}

internal class DiceSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld ecsWorld;

    private EcsFilter<DiceClick> filter;

    public void Init()
    {
        DiceView.onDiceClicked += () =>
        {
            EcsEntity entity = ecsWorld.NewEntity();
            entity.Get<DiceClick>();
        };
    }

    public void Run()
    {
        if (!filter.IsEmpty())
        {
            var entity = filter.GetEntity(0);
            DiceRoll roll = new DiceRoll {roll = Random.Range(0, 7)};
            entity.Replace(roll);
        }
    }
}

public struct DiceRoll
{
    public int roll;
}

public struct DiceClick
{
}

internal class CardInputSystem : IEcsRunSystem
{
    private EcsFilter<SwipeDirection> swipeFilter;
    private PointsSystem pointsSystem;
    private GameContext gameContext;

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

            if (currentCard.Has<SkillsLeftRight>())
            {
                SkillsLeftRight skillsLeftRight = currentCard.Get<SkillsLeftRight>();
                SkillsComponent skillsComponent = skillsLeftRight.GetSwipe(swipeDirection);
                SkillsSystem.Instance.CreateSkillsUpdate(skillsComponent);
            }
            else if (currentCard.Has<PointsLeftRight>() && !currentCard.Has<SkillsCheck>())
            {
                PointsLeftRight pointsLeftRight = currentCard.Get<PointsLeftRight>();
                PointsComponent pointsComponent = pointsLeftRight.GetSwipe(swipeDirection);
                PointsSystem.ChangePoints(pointsComponent);
            }
            else if (currentCard.Has<PointsLeftRight>() && currentCard.Has<SkillsCheck>())
            {
                
            }
            else
            {
                Debug.LogError("Swiped but current card is not a swipe card or skill choose caed");
                return;
            }

            Debug.Log("Destroying card " + currentCard);
            currentCard.Destroy();
        }
    }
}

internal class CardsSystem : IEcsRunSystem
{
    private EcsWorld ecsWorld;

    // ну и что вот у нас получается куча карт cards[] - какая из них текущая?
    // так ну что лучше по одному ивенту на каждый тип карты? Криво. Или отсюда короче напрямую вызывать
    // лол походу пришло время писать криво и костыли ибо чето опять же хз как сделать этот паттерн - потом увидим.
    // либо ивент делать не просто из функции а передаввать уишке весь стейт за счет функции хммм но короче потом
    private EcsFilter<CardInfo> filter;
    private GameContext gameContext;

    public void Run()
    {
        if (!gameContext.currentCard.HasValue || !gameContext.currentCard.Value.IsAlive())
        {
            var cardEntity = filter.GetEntity(0);

            if (!cardEntity.IsAlive())
            {
                Debug.LogError("Card entity " + cardEntity + " is not alive");
            }

            gameContext.currentCard = cardEntity;
            ref CardInfo cardInfo = ref cardEntity.Get<CardInfo>();

            //todo unify card check logic
            if (cardEntity.Has<PointsLeftRight>() && !cardEntity.Has<SkillsCheck>())
            {
                CardUI.Instance.ShowCardData(cardEntity, cardInfo, cardEntity.Get<PointsLeftRight>());
            }
            else if (cardEntity.Has<SkillsLeftRight>())
            {
                CardUI.Instance.ShowCardData(cardEntity, cardInfo, cardEntity.Get<SkillsLeftRight>());
            }
            else if (cardEntity.Has<PointsLeftRight>() && cardEntity.Has<SkillsCheck>())
            {
                CardUI.Instance.ShowCardData(cardEntity, cardInfo,
                    cardEntity.Get<PointsLeftRight>(), cardEntity.Get<SkillsCheck>());
            }
            else if (cardEntity.Has<Trade>())
            {
                TradeUI.Instance.ShowCard(cardEntity.Get<Trade>());
            }
            else
            {
                Debug.LogError("This card is not known type " + cardEntity);
            }
        }
    }
}

internal class CardsInitSystem : IEcsInitSystem
{
    private EcsWorld ecsWorld;

    public void Init()
    {
        CreatePointsCard(new PointsLeftRight
            {left = new PointsComponent {food = 1}, right = new PointsComponent {food = -1}});
        CreatePointsCard(new PointsLeftRight
            {left = new PointsComponent {hull = 1}, right = new PointsComponent()});
        CreatePointsCard(new PointsLeftRight
            {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}});

        CreateSkillsCard(new SkillsLeftRight
            {left = new SkillsComponent {fighting = 1}, right = new SkillsComponent {science = 1}});
        CreateSkillsCard(new SkillsLeftRight
            {left = new SkillsComponent {charisma = 1}, right = new SkillsComponent {survival = 1}});
        CreateSkillsCard(new SkillsLeftRight
            {left = new SkillsComponent {mechanical = 1}, right = new SkillsComponent {fighting = 1}});

        CreateSkillsCheckCard(new PointsLeftRight
                {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}},
            new SkillsCheck {skillsToCheck = new SkillsComponent {mechanical = 1}});
        CreateSkillsCheckCard(new PointsLeftRight
                {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}},
            new SkillsCheck {skillsToCheck = new SkillsComponent {charisma = 1}});
        CreateSkillsCheckCard(new PointsLeftRight
                {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}},
            new SkillsCheck {skillsToCheck = new SkillsComponent {fighting = 1}});

        CreateTradeCard(new Trade
        {
            skillsComponents =
                new[]
                {
                    new SkillsComponent {fighting = 1},
                    new SkillsComponent {survival = 1},
                    new SkillsComponent {science = 1}
                },
            skillComponentsCosts = new[] {1, 1, 1},
            foodCost = 1, hullCost = 1
        });
    }

    private EcsEntity CreatePointsCard(PointsLeftRight pointsLeftRight)
    {
        var cardEntity = CreateCard();
        cardEntity.Replace(pointsLeftRight);
        return cardEntity;
    }

    private EcsEntity CreateSkillsCard(SkillsLeftRight skillsLeftRight)
    {
        EcsEntity cardEntity = CreateCard();
        cardEntity.Replace(skillsLeftRight);
        return cardEntity;
    }

    private EcsEntity CreateSkillsCheckCard(PointsLeftRight pointsLeftRight, SkillsCheck skillsCheck)
    {
        EcsEntity cardEntity = CreateCard();
        cardEntity.Replace(pointsLeftRight);
        cardEntity.Replace(skillsCheck);
        return cardEntity;
    }

    private EcsEntity CreateTradeCard(Trade trade)
    {
        EcsEntity cardEntity = CreateCard();
        cardEntity.Replace(trade);
        
        return cardEntity;
    }
    
       
    private EcsEntity CreateCard()
    {
        EcsEntity cardEntity = ecsWorld.NewEntity();
        cardEntity.Get<CardInfo>();
        return cardEntity;
    }
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