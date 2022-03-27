using System;
using Client;
using MyBox;
using MyBox.Internal;
using UnityEngine;

[CreateAssetMenu]
public class CardObject : ScriptableObject
{
    public CardType cardType;

    /**
     * textовую тему сюда просто и будет карта которая отображает просто текст
     * и тоесть вопрос а собсно делатьб это через лефт райт или делать это через отдельную карту
     */
    public string text;

    [ConditionalField("cardType", false, CardType.SwipePoints, CardType.SkillCheck)]
    public PointsLeftRight pointsLeftRight;

    [ConditionalField("cardType", false, CardType.SwipeSkills)]
    public SkillsLeftRight skillsLeftRight;

    [ConditionalField("cardType", false, CardType.SkillCheck)]
    public SkillsCheck skillsCheck;

    [ConditionalField("cardType", false, CardType.Trade)]
    public Trade trade;
}

[Serializable]
public struct CardTest
{
    public Optional<PointsComponent> points;
    public Optional<SkillsComponent> skillsComponent;
    public Optional<SkillsCheck> skillsCheck;
    public Optional<Trade> tradeTest;
}

[Serializable]
public class Optional<T>
{
    public bool IsSet;

    [ConditionalField("IsSet", false, true)]
    public T Value;
}