using Client;
using Leopotam.Ecs;

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
            pointsComponents = new []
            {
                new PointsComponent() {food = 1},
                new PointsComponent() {hull = 1},
            }, 
            pointsComponentsCosts = new [] {1, 1}
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