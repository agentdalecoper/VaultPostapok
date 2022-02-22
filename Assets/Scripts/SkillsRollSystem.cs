using Leopotam.Ecs;
using UnityEngine;

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