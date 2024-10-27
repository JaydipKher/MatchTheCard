using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionManager : MonoBehaviour
{
    public static ActionManager instance;
    public Action<int> generateLevel;

    private void Awake()
    {
        instance = this;
    }
}
