
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ComponentName(LogicComponentType.Action, "数值/数值计算")]
[System.Serializable]
public class Action_Value_Control : ActionComponentBase
{
    [InternalValueMenu(new Type[] { typeof(float) })]
    public string resultValueName = "";
    public string formula = "";
    protected override void Init()
    {
        base.Init();
    }

    protected override void Action()
    {
        List<BaseValue> internalVariableList = logicObject.logicManager.data.internalValueList;
        string tempFormula = formula;
        
        for (int i = 0; i < internalVariableList.Count; i++)
        {
            string valueStr =logicObject.logicManager.GetInternalValue(internalVariableList[i].name).ToString();
            string temp = tempFormula.Replace("{" + internalVariableList[i].name + "}", valueStr);
           if (!temp.Equals(tempFormula))
           {
               tempFormula = temp;
           }
        }
        try
        {
            //Debug.Log("Formula:" + tempFormula);
            object c = CalculateTool.CalculateExpression(tempFormula);//1+12+3
            logicObject.logicManager.SetInternalValue(resultValueName, c);          
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public override string ToExplain()
    {
        return "公式说明例如：结果变量=(1+2)*(12/3%3)+{test0}*20,({test0}为内部变量)";
    }
}
