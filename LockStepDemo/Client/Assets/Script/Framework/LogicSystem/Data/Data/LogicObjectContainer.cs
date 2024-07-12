using System;
using System.Collections.Generic;

[System.Serializable]
public class LogicObjectContainer
{
    private LogicRuntimeMachine logicManager;
    //储存内部变量
    public List<BaseValue> internalValueList = new List<BaseValue>();

    public List<LogicObject> logicObjs = new List<LogicObject>();
    public int startId = 0;
    public void Init(LogicRuntimeMachine logicManager)
    {
        this.logicManager = logicManager;
        for (int i = 0; i < logicObjs.Count; i++)
        {
            LogicObject to = logicObjs[i];
            if (to.id == startId)
                to.Init(logicManager);
            else if (to.isSupportAlwaysActive)
            {
                to.Init(logicManager);
            }
        }
    }
    public LogicObject GetLogicObject(int id)
    {
        for (int i = 0; i < logicObjs.Count; i++)
        {
            if (logicObjs[i].id == id)
                return logicObjs[i];
        }

        return null;
    }

    public BaseValue GetBaseValue(string name)
    {
        for (int i = 0; i < internalValueList.Count; i++)
        {
            if (internalValueList[i].name == name)
                return internalValueList[i];
        }
        return null;
    }
    /// <summary>
    /// 根据Type名字获取对应类型的内部变量的名字
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public List<string> GetInternalValueNamesByTypes(string[] types = null)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < internalValueList.Count; i++)
        {
            BaseValue bv = internalValueList[i];
            if (types == null)
            {
                list.Add(bv.name);
            }
            else
            {
                for (int j = 0; j < types.Length; j++)
                    if (bv.typeName == types[j])
                        list.Add(bv.name);
            }
        }
        return list;
    }
    public void Close()
    {
        for (int i = 0; i < logicObjs.Count; i++)
        {
            logicObjs[i].Close();
        }
        logicObjs.Clear();
        internalValueList.Clear();
    }
}
