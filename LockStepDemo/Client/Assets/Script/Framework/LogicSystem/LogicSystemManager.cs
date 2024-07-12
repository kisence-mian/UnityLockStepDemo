using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogicSystemManager  {

    public static CallBack<int, int> NetworkSendActionIdCallback;

    private static int idCounter = 0;
    private static int GetId
    {
        get
        {
            idCounter++;
            return idCounter;
        }
    }
    private static Dictionary<int, LogicRuntimeMachine> logicMachineDic = new Dictionary<int, LogicRuntimeMachine>();

    public static LogicRuntimeMachine NewLogicRuntimeMachine(string fileName)
    {
        LogicObjectContainer container = LogicObjectDataController.GetDataFromFile(fileName);
        int id = GetId;
        LogicRuntimeMachine lom = new LogicRuntimeMachine(id, container);
        logicMachineDic.Add(id, lom);
        return lom;
    }

    public static LogicRuntimeMachine GetLogicRuntimeMachine(int id)
    {
        if (logicMachineDic.ContainsKey(id))
            return logicMachineDic[id];
        return null;
    }
    public static void PauseLogicRuntimeMachine(int id, bool isPause)
    {
        if (logicMachineDic.ContainsKey(id))
        {
            logicMachineDic[id].Pause(isPause);            
        }    
    }
    public static void PauseAllLogicRuntimeMachine(bool isPause)
    {
        List<LogicRuntimeMachine> list = new List<LogicRuntimeMachine>(logicMachineDic.Values);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Pause(isPause);
        }
    }
    public static void StopLogicRuntimeMachine(int id)
    {
        if (logicMachineDic.ContainsKey(id))
        {
            logicMachineDic[id].Close();
            logicMachineDic.Remove(id);
        }
    }
    public static void StopAllLogicRuntimeMachine()
    {
        List<LogicRuntimeMachine> list = new List<LogicRuntimeMachine>(logicMachineDic.Values);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Close();
        }
        logicMachineDic.Clear();
    }

    public static void NetworkSendActionId(int machineId,int actionId)
    {
        if (NetworkSendActionIdCallback != null)
            NetworkSendActionIdCallback(machineId, actionId);
        else
            NetworkReceiveActionId(machineId, actionId);
    }
    public static void NetworkReceiveActionId(int machineId, int actionId)
    {
        Debug.Log("执行动作：machineId：" + machineId + "  actionId:" + actionId);
        logicMachineDic[machineId].RunActions(actionId);
    }
}
