using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class LogicSystemWindowUpdater : EditorMonoBehaviour
{
    private double updateTime = 1;
    private double temp = 0;
    public override void Update()
    {
        if (temp <= 0)
        {
            temp = updateTime;

            LogicSystemEditorWindow.UpdateStateDataInfo();
        }
        else
        {
            temp -= deltaTime;
        }
    }

  
}
