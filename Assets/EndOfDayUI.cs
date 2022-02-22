using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndOfDayUI : MonoBehaviour
{
    private static EndOfDayUI instance;
    public static EndOfDayUI Instance => instance;

    public TextMeshProUGUI endOfDayText;
    public Button button;


    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }


    public void SetData(int dayNumber)
    {
        endOfDayText.text = $"End of day {dayNumber}";
    }
}