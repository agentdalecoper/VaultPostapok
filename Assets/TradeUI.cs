using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeUI : MonoBehaviour
{
    private static TradeUI instance;
    public static TradeUI Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowCard(Trade trade)
    {
        
    }
}
