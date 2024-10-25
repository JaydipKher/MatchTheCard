using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionManager : MonoBehaviour
{
    public static ActionManager instance;
    public Action<int> levelGenerator;

    private void Awake()
    {
        instance = this;
    }
}
