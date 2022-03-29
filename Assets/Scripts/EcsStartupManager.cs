using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;    
using Client;
using Leopotam.Ecs;
using MyBox.Internal;
using SwipeableView;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public DialogNodeCanvas dayCanvas;
    
    
    [Obsolete]
    public CardObject[] cardsObjects;
}

sealed class EcsStartupManager : MonoBehaviour
{
    EcsWorld _world;
    EcsSystems _systems;
    public SceneConfiguration sceneConfiguration;
    public DialogNodeCanvas canvas;

    void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);


#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif

        // создаем currentCard ecsentity, проставляем ему все и отображаем пользователю, калкулейтим эффекты
        // если вправо то вправо, если влево то влево
        /*
         * так - а как вообще должны карты подтягиваться из конфигурации
         * так ну это либо scriptable objects которые наследуются от интерфейса
         *
         *
         * Как инициализировать класс?
         *      - мы создаем карту на лету CardInfo - после пользовательского ввода или другого ивента
         *      - после того как юзер ввел инпут - создается следующая карта 
         *      
         * CardInfo
         *      cardNode
         *
         * проставляем на фронт тоже
         *      влево вправо
         *      альтернатива - из всего сделать ecsentity и референс структуру но пока давай так
         *
         * короче я бы попробовал сделать тогда именно так - как описал, через таки создание ECS entities и так далее
         * да короче - создать ecs компоненты над существующими и проставить дерево таким образом
         * после этого сделать уже компонент игры
         * 
         */
        GameContext gameContext = new GameContext();
        PointsSystem pointsSystem = new PointsSystem();
        _systems
            .Add(new DaySystem())
            .Add(new NodeSystem())
            // .Add(new CardsInitSystem())
            .Add(new CardInputSystem())
            // .Add(new NextCardSystem())
            .Add(new CardsMechanicsSystem())
            // .Add(new DiceSystem())
            .Add(pointsSystem)
            .Add(new SwipeSystem())
            .Add(new SkillsSystem())
            .Add(new SkillsCheckSystem())
            .Add(new TradeSystem())
            .Add(new CardsViewSystem())
            .Add(new EndOfDaySystem())
            .Add(new EndOfGameSystem())
            .Inject(sceneConfiguration)
            .Inject(gameContext)
            .Inject(pointsSystem)
            .Inject(canvas)
            .OneFrame<SwipeDirection>()
            .OneFrame<DiceClick>()
            .OneFrame<DiceRoll>()
            .OneFrame<CreateCard>()
            .OneFrame<EndOfDay>()
            .OneFrame<Render>()
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

internal class CardsMechanicsSystem : IEcsRunSystem
{
    private EcsFilter<CardInfo, Render> renderFilter;

    public void Run()
    {
        if (!renderFilter.IsEmpty())
        {
            EcsEntity cardEntity = renderFilter.GetEntity(0);
            ref CardInfo cardInfo = ref cardEntity.Get<CardInfo>();

            if (cardEntity.Has<SkillsComponent>())
            {
                SkillsSystem.Instance.CreateSkillsUpdate(cardEntity.Get<SkillsComponent>());
            }

            if (cardEntity.Has<PointsComponent>())
            {
                PointsSystem.ChangePoints(cardEntity.Get<PointsComponent>());
            }

            if (cardEntity.Has<SkillsCheck>())
            {
                DiceRoll roll = new DiceRoll {roll = Random.Range(0, 7)};
                cardEntity.Replace(roll);
            }
            
            if (cardEntity.Has<Trade>())
            {
            }
        }
    }
}

internal class NextCardSystem : IEcsRunSystem
{
    private GameContext gameContext;

    public void Run()
    {
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
            cardEntity.Get<Render>();
        }   
    }
}

internal class NodeSystem : IEcsRunSystem
{
    private EcsWorld ecsWorld;
    private EcsFilter<Day, CreateCard> dayCreateCardsFilter;
    private GameContext gameContext;

    public void Run()
    {
        foreach (int i in dayCreateCardsFilter)
        {
            ref Day day = ref dayCreateCardsFilter.Get1(i);
            gameContext.dayCards = new List<EcsEntity>();
            DialogNodeCanvas canvas = day.dayCanvas;
            
            IEnumerable<CardNode> startNodes = canvas
                .nodes
                .OfType<CardNode>()
                .OrderBy(node => node.rect.y)
                .Where(node => node.frinPreviousIN.connections.Count == 0);
        
            foreach (CardNode startNode in startNodes)
            {
                EcsEntity cardEntity = CreateCard(startNode);
                Debug.Log("Start node card " + cardEntity + " " + cardEntity.Get<CardInfo>().cardNode.CharacterName);
                Debug.Log("Start node child count " + cardEntity.Get<CardInfo>().nextCards.Count);
                gameContext.dayCards.Add(cardEntity);
            }
        }
    }

    // todo implement better recursion here
    private EcsEntity CreateCard(CardNode node)
    {
        Debug.Log("Start node " + node.DialogLine + " " + node.CharacterName);
        EcsEntity entity = ecsWorld.NewEntity();
        setCardInfo(node, entity);

        ref var cardInfo = ref entity.Get<CardInfo>();

        for (var i = 0; i < node._options.Count; i++)
        {
            DialogOption dialogOption = node._options[i];

            if (node.dynamicConnectionPorts.Count == 0)
            {
                break;
            }

            if (!node.dynamicConnectionPorts[i].connected())
            {
                var cardStub = ecsWorld.NewEntity();
                cardStub.Get<CardStub>();
                cardInfo.nextCards.Add(cardStub);
                cardStub.Replace(dialogOption);
                continue;
            }
            
            CardNode childNode = node.dynamicConnectionPorts[i].connection(0).body as CardNode;
            EcsEntity childEntity = CreateCard(childNode);
            ref var childCardInfo = ref childEntity.Get<CardInfo>();
            childEntity.Replace(dialogOption);
            
            cardInfo.nextCards.Add(childEntity);
        }

        return entity;
    }

    private CardInfo setCardInfo(CardNode node, EcsEntity entity)
    {
        ref CardInfo cardInfo = ref entity.Get<CardInfo>();
        cardInfo.sprite = node.CharacterPotrait;
        cardInfo.text = node.DialogLine;
        cardInfo.cardNode = node;
        cardInfo.nextCards = new List<EcsEntity>();
        cardInfo.actionsOnEnter = node.actionsOnEnter;
        cardInfo.audioClip = node.SoundDialog;
        if (node.cardObject.points.IsSet)
        {
            entity.Replace(node.cardObject.points.Value);
        }
        if (node.cardObject.skillsCheck.IsSet)
        {
            entity.Replace(node.cardObject.skillsCheck.Value);
        }
        if (node.cardObject.skillsComponent.IsSet)
        {
            entity.Replace(node.cardObject.skillsComponent.Value);
        }
        if (node.cardObject.tradeTest.IsSet)
        {
            entity.Replace(node.cardObject.tradeTest.Value);
        }
        
        return cardInfo;
    }
}

public struct CardStub
{
}

internal struct CreateCard
{
}

public struct DiceRoll
{
    public int roll;
    public bool success;
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
    public EndOfGame? endOfGame;
    public SkillsComponent playerSkills;
}

public struct CardInfo
{
    public string text;
    public Sprite sprite;
    public CardType cardType;
    public List<EcsEntity> nextCards;
    public DialogAction[] actionsOnEnter;

    public AudioClip audioClip;
    public CardNode cardNode;
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