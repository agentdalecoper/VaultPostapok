using System;
using TMPro;
using UnityEngine;

public class HeaderUi : MonoBehaviour
{
    public TextMeshProUGUI money;
    public TextMeshProUGUI foods;
    public TextMeshProUGUI hull;
    public TextMeshProUGUI workForce;
    
    public TextMeshProUGUI dateText;
    
    private static HeaderUi instacne;
    public static HeaderUi Instance => instacne;

    private void Awake()
    {
        instacne = this;
    }

}