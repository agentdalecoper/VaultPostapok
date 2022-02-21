using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Button buttonLeft;
    public Button buttonRight;
    public TextMeshProUGUI text;

    public static event Action<EcsEntity> ActionNewCardAppeared;
    public static event Action<EcsEntity> ActionSwipedRight;
    public static event Action<EcsEntity> ActionSwipedLeft;

    private static CardUI instance;
    public static CardUI Instance => instance;

    private void Awake()
    {
        instance = this;

        buttonLeft.onClick.AddListener(delegate { ActionSwipedLeft?.Invoke(EcsEntity.Null); });
        buttonRight.onClick.AddListener(delegate { ActionSwipedRight?.Invoke(EcsEntity.Null); });
    }

    public void ShowCardData(EcsEntity cardEntity, CardInfo cardInfo, PointsLeftRight pointsLeftRight)
    {
        text.text = " Left points: " + pointsLeftRight.left + " Right points: " + pointsLeftRight.right;
    }

    public void ShowCardData(EcsEntity cardEntity, CardInfo cardInfo, SkillsLeftRight skillsLeftRight)
    {
        text.text = " Left skill: " + skillsLeftRight.left + " Right skill: " + skillsLeftRight.right;
    }
}