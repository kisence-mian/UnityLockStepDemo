using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class LogicComponentBase  {
  
    protected LogicObject logicObject;

    [NoShowInEditor]
    public List<InternalValueInfo> internalValueInfoList = new List<InternalValueInfo>();
    /// <summary>
    /// 初始化使用内部变量
    /// </summary>
    protected void UpdateInternalValue()
    {
        if (internalValueInfoList==null || internalValueInfoList.Count == 0)
            return;
       Type t = GetType();
       FieldInfo[] fi= t.GetFields();
       for (int i = 0; i < internalValueInfoList.Count; i++)
       {
           InternalValueInfo info = internalValueInfoList[i];
           object value = logicObject.logicManager.GetInternalValue(info.internalValueName);
           for (int j = 0; j < fi.Length; j++)
           {
               if (fi[j].Name == info.fieldName)
               {
                   fi[j].SetValue(this, value);
               }
           }
       }

    }
    public InternalValueInfo GetInternalValueInfoByFieldName(string fieldName)
    {
        if (internalValueInfoList == null)
            internalValueInfoList = new List<InternalValueInfo>();
        for (int i = 0; i < internalValueInfoList.Count; i++)
        {
            if (internalValueInfoList[i].fieldName == fieldName)
                return internalValueInfoList[i];
        }
        return null;
    }
    public void Initialize(LogicObject logicObject)
    {
        this.logicObject = logicObject;
        UpdateInternalValue();
        Init();
    }
    protected virtual void Init() { }
    public virtual void OnPause(bool isPause) { }
    public virtual void OnClose() { }
    public virtual string ToExplain()
    {
        return "Explain trigger";
    }
}
[System.Serializable]
public class InternalValueInfo
{
    public string fieldName="";
    public string internalValueName = "";
}
