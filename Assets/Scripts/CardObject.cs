using MyBox;
using UnityEngine;

[CreateAssetMenu]
public class CardObject : ScriptableObject
{
    public CardType cardType;

    [ConditionalField("cardType", false, CardType.SwipePoints, CardType.SkillCheck)]
    public PointsLeftRight pointsLeftRight;

    [ConditionalField("cardType", false, CardType.SwipeSkills)]
    public SkillsLeftRight skillsLeftRight;

    [ConditionalField("cardType", false, CardType.SkillCheck)]
    public SkillsCheck skillsCheck;

    [ConditionalField("cardType", false, CardType.Trade)]
    public Trade trade;
}