using System.Collections.Generic;
using Client;
using Leopotam.Ecs;

internal class CardsInitSystem : IEcsInitSystem
{
    private EcsWorld ecsWorld;
    private GameContext gameContext;

    public void Init()
    {
        gameContext.dayCards = new List<EcsEntity>()
        {
            CreatePointsCard(new PointsLeftRight
                {left = new PointsComponent {food = 1}, right = new PointsComponent {food = -1}}),
            CreatePointsCard(new PointsLeftRight
                {left = new PointsComponent {hull = 1}, right = new PointsComponent()}),
            CreatePointsCard(new PointsLeftRight
                {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}}),
            CreateSkillsCard(new SkillsLeftRight
                {left = new SkillsComponent {fighting = 1}, right = new SkillsComponent {science = 1}}),
            CreateSkillsCard(new SkillsLeftRight
                {left = new SkillsComponent {charisma = 1}, right = new SkillsComponent {survival = 1}}),
            CreateSkillsCard(new SkillsLeftRight
                {left = new SkillsComponent {mechanical = 1}, right = new SkillsComponent {fighting = 1}}),

            CreateSkillsCheckCard(new PointsLeftRight
                    {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}},
                new SkillsCheck {skillsToCheck = new SkillsComponent {mechanical = 1}}),
            CreateSkillsCheckCard(new PointsLeftRight
                    {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}},
                new SkillsCheck {skillsToCheck = new SkillsComponent {charisma = 1}}),
            CreateSkillsCheckCard(new PointsLeftRight
                    {left = new PointsComponent {hull = 1}, right = new PointsComponent() {food = 1}},
                new SkillsCheck {skillsToCheck = new SkillsComponent {fighting = 1}}),
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
                pointsComponents = new[]
                {
                    new PointsComponent() {food = 1},
                    new PointsComponent() {hull = 1},
                },
                pointsComponentsCosts = new[] {1, 1}
            })
        };
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
}