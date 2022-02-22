using Client;
using Leopotam.Ecs;

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