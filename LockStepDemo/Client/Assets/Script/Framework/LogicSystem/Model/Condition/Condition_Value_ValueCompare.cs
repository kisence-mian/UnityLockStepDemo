using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ComponentName(LogicComponentType.Condition, "数值/数值比较")]
[System.Serializable]
public class Condition_Value_ValueCompare : ConditionComponentBase {
    public string internalValueName = "";
    public string compareType = "Equal";
    
    public bool useInternalValue = false;
    public string compareInternalValueName = "";
    public BaseValue compareBaseValue;
    protected override void Init()
    {
        base.Init();
    }
    protected override bool Compare(params object[] objs)
    {
       object value = logicObject.logicManager.GetInternalValue(internalValueName);
       object value1 = null;
       if (useInternalValue)
       {
           value1 = logicObject.logicManager.GetInternalValue(compareInternalValueName);
       }
       else
       {
           value1 = compareBaseValue.GetValue();
       }

       return CompareValue.Compare(value,value1,compareType);
    }
}
