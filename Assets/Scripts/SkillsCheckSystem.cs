using Leopotam.Ecs;
using UnityEngine;

internal class SkillsCheckSystem : IEcsRunSystem
{
    private EcsFilter<DiceRoll> filter;
    private GameContext gameContext;

    public void Run()
    {
        foreach (int i in filter)
        {
            ref DiceRoll diceRoll =ref  filter.Get1(i);

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

            // if (!cardEntity.Has<PointsLeftRight>())
            // {
            //     Debug.LogError("Dice rolled - but card has no PointsLeftRight");
            //     return;
            // }

            SkillsCheck skillsCheck =  cardEntity.Get<SkillsCheck>();
            bool success = skillsCheck.skillsToCheck.fighting < diceRoll.roll + gameContext.playerSkills.fighting &&
                           skillsCheck.skillsToCheck.science < diceRoll.roll + gameContext.playerSkills.science &&
                           skillsCheck.skillsToCheck.mechanical < diceRoll.roll + gameContext.playerSkills.mechanical &&
                           skillsCheck.skillsToCheck.survival < diceRoll.roll + gameContext.playerSkills.survival &&
                           skillsCheck.skillsToCheck.charisma < diceRoll.roll + gameContext.playerSkills.charisma;

            diceRoll.success = success;

            // PointsLeftRight pointsLeftRight = cardEntity.Get<PointsLeftRight>();
            // if (!success)
            // {
            //     PointsSystem.ChangePoints(pointsLeftRight.left);
            // }
            // else
            // {
            //     PointsSystem.ChangePoints(pointsLeftRight.right);
            // }
        }
    }
}