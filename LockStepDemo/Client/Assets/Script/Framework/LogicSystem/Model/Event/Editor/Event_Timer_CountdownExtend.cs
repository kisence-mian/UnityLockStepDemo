using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[EditorExtend(typeof(Event_Timer_Countdown))]
public class Event_Timer_CountdownExtend : EditorExtendBase
{
    public override void EditorOverrideClassGUI(object value)
    {
        Event_Timer_Countdown ec = (Event_Timer_Countdown)value;
        ec.timerName = EditorDrawGUIUtil.DrawBaseValue("计时器名字", ec.timerName).ToString();
        ec.timeCount = (float)EditorDrawGUIUtil.DrawBaseValue("总时间", ec.timeCount);
        ec.isIgnoreTimeScale = (bool)EditorDrawGUIUtil.DrawBaseValue("时间缩放", ec.isIgnoreTimeScale);
        GUILayout.Space(5);
        ec.isSetTimeValue2InternalValue = (bool)EditorDrawGUIUtil.DrawBaseValue("使用内部变量记录剩余时间", ec.isSetTimeValue2InternalValue);

        if (ec.isSetTimeValue2InternalValue)
        {
            ec.internalVariableName = LogicSystemAttributeEditorGUI.DrawInternalValueMenu("内部变量名", ec.internalVariableName, new string[] { typeof(int).FullName}).ToString();
        }
       
    }
}
