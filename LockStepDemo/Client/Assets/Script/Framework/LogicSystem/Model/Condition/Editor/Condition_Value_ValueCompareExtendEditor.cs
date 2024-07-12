using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[EditorExtend(typeof(Condition_Value_ValueCompare))]
public class Condition_Value_ValueCompareExtendEditor : EditorExtendBase
{
    public override void EditorOverrideClassGUI(object value)
    {
        Condition_Value_ValueCompare data = (Condition_Value_ValueCompare)value;

        data.internalValueName = LogicSystemAttributeEditorGUI.DrawInternalValueMenu("内部变量名", data.internalValueName,null).ToString();
        if (string.IsNullOrEmpty(data.internalValueName))
            return;
       object v=  LogicSystemEditorWindow.GetInternalValue(data.internalValueName);
       List<string> compareNames=  CompareValue.GetCompareType(v.GetType());
        data.compareType = EditorDrawGUIUtil.DrawPopup("比较类型", data.compareType, compareNames);

        data.useInternalValue = (bool)EditorDrawGUIUtil.DrawBaseValue("使用内部变量", data.useInternalValue);

        if (data.useInternalValue)
        {
          data.compareInternalValueName=  LogicSystemAttributeEditorGUI.DrawInternalValueMenu( "相比较的内部变量名", data.compareInternalValueName,new string[] { v.GetType().FullName}).ToString();          
        }
        else
        {
            if (data.compareBaseValue == null)
                data.compareBaseValue = new BaseValue("", v);
            if (data.compareBaseValue.typeName != v.GetType().FullName)
                data.compareBaseValue.SetValue("", v);

            object temp = EditorDrawGUIUtil.DrawBaseValue("比较值("+data.compareBaseValue.typeName+")", data.compareBaseValue.GetValue());
            data.compareBaseValue.SetValue("", temp);
        }
    }
}
