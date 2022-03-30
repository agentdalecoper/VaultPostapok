using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    
    private static StartGameUI instance;
    public static StartGameUI Instance => instance;

    public void Awake()
    {
        instance = this;
    }
}
