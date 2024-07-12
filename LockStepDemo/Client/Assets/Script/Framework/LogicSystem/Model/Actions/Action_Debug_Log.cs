using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ComponentName(LogicComponentType.Action, "日志/Log")]
[System.Serializable]
public class Action_Debug_Log : ActionComponentBase
{
    //是否是打印内部变量
    public bool isLogInternalVariable = false;
    //内部变量名字
    public string internalVariableName = "";
    public string message = "";
     public LogType logType = LogType.Log;
    protected override void Action()
    {
        if (isLogInternalVariable)
        {
           object value =  logicObject.logicManager.GetInternalValue(internalVariableName);
            Debug.logger.Log(logType,"log组件："+ value);
        }
        else
        {
            Debug.logger.Log(logType, "log组件：" + message);
        }
    }

    public override string ToExplain()
    {
        string ss = "打印Log：";
        if (isLogInternalVariable)
        {
            ss += "内部变量名：" + internalVariableName;
        }
        else
        {
            ss += message;
        }
        return ss;
    }
}
