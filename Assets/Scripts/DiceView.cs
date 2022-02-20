using System.Collections;
using System.Collections.Generic;
using SwipeableView;
using UnityEngine;

public class DiceView : MonoBehaviour
{
    private static DiceView instance;
    public static DiceView Instance => instance;
    public UISwipeableCardBasic uISwipeableCardBasic;

    public void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
}
