using System;
using System.Collections;
using System.Collections.Generic;
using SwipeableView;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceView : MonoBehaviour, IPointerClickHandler
{
    private static DiceView instance;
    public static DiceView Instance => instance;
    public static event Action onDiceClicked;

    public TextMeshProUGUI text;

    public bool diceEnabled = true;

    public void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!diceEnabled)
        {
            return;
        }
        
        Debug.Log("Clicked dice " + eventData);
        
        onDiceClicked?.Invoke();
    }
}
