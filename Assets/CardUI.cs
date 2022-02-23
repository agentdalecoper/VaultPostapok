using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;
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
    public DiceView diceView;

    TaskCompletionSource<bool> isWaitingUiDelay = new TaskCompletionSource<bool>();

    public static event Action<EcsEntity> ActionNewCardAppeared;
    public static event Action<EcsEntity> ActionSwipedRight;
    public static event Action<EcsEntity> ActionSwipedLeft;

    private static CardUI instance;
    public static CardUI Instance => instance;

    private void Awake()
    {
        instance = this;
        
        isWaitingUiDelay.SetResult(false);

        buttonLeft.onClick.AddListener(delegate { ActionSwipedLeft?.Invoke(EcsEntity.Null); });
        buttonRight.onClick.AddListener(delegate { ActionSwipedRight?.Invoke(EcsEntity.Null); });
        
        diceView.gameObject.SetActive(false);
    }

    public async void ShowCardData(EcsEntity cardEntity, CardInfo cardInfo, PointsLeftRight pointsLeftRight)
    {
        await isWaitingUiDelay.Task;
        
        DeActivateDiceView();
        text.text = " Left points: " + pointsLeftRight.left + " Right points: " + pointsLeftRight.right;
    }

    public async void ShowCardData(EcsEntity cardEntity, CardInfo cardInfo, SkillsLeftRight skillsLeftRight)
    {
        await isWaitingUiDelay.Task;

        DeActivateDiceView();
        text.text = " Left skill: " + skillsLeftRight.left + " Right skill: " + skillsLeftRight.right;
        
    }

    public async void ShowCardData(EcsEntity cardEntity, CardInfo cardInfo, PointsLeftRight pointsLeftRight,
        SkillsCheck skillsCheck)
    {
        await isWaitingUiDelay.Task;

        ActivateDiceView();
        text.text = " Skills to check" + skillsCheck.skillsToCheck
                                       + " Left points: " + pointsLeftRight.left + " Right points: " +
                                       pointsLeftRight.right;
        diceView.text.text = 0.ToString();
    }
    
    
    public async void ShowDiceData(DiceRoll diceRoll, bool success,
        SkillsCheck skillsCheck, SkillsComponent playerSkills)
    {
        diceView.diceEnabled = false;

        if (success)
        {
            diceView.GetComponent<Image>().color = Color.green;
        }
        else
        {
            diceView.GetComponent<Image>().color = Color.red;
        }
        
        isWaitingUiDelay.TrySetResult(true);
        diceView.text.text = diceRoll.roll.ToString();
                
        buttonLeft.gameObject.SetActive(true);
        buttonRight.gameObject.SetActive(true);

        string successOrLoose = success ? "Dice success" : "Dice lose";
        text.text = $"{successOrLoose} was checking {skillsCheck} vs. " +
                    $" playerSkills={playerSkills} + diceRoll={diceRoll.roll}";

        await Task.Delay(TimeSpan.FromSeconds(20f));
        isWaitingUiDelay.TrySetResult(false);
    }

    private void ActivateDiceView()
    {
        diceView.gameObject.SetActive(true);
        diceView.diceEnabled = true;
        diceView.GetComponent<Image>().color = Color.white;

        diceView.gameObject.SetActive(true);
        buttonLeft.gameObject.SetActive(false);
        buttonRight.gameObject.SetActive(false);
    }

    private void DeActivateDiceView()
    {
        diceView.gameObject.SetActive(false);
        buttonLeft.gameObject.SetActive(true);
        buttonRight.gameObject.SetActive(true);
    }
}