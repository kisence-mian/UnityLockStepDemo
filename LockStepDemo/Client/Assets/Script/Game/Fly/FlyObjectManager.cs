using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlyObjectManager 
{
    static List<FlyObjectBase> s_flyList = new List<FlyObjectBase>();

    public static void ReceviceCmd(CommandBase cmd)
    {

    }

    public static void CreateFlyObject(string flyName,int id,string skillID, int createrID,Vector3 pos ,Vector3 dir)
    {
        Debug.Log("CreateFlyObject flyName"+ flyName+"  " + pos + " dir " + dir);

        string modleName = null;
        try
        {
            FlyDataGenerate property = DataGenerateManager<FlyDataGenerate>.GetData(flyName);
            modleName = property.m_ModelName;
            GameObject fly = GameObjectManager.CreateGameObjectByPool(property.m_ModelName);
            FlyObjectBase flyObj = fly.GetComponent<FlyObjectBase>();

            if(flyObj == null)
            {
                Debug.LogError(property.m_ModelName + " not is flyobj");
            }

            flyObj.m_property = property;

            flyObj.Init(id, skillID, createrID, flyName, pos, dir);

            s_flyList.Add(flyObj);
        }
        catch(Exception e)
        {
            Debug.LogError("CreateFlyObject error :flyName: ->" + flyName + "<- m_ModelName：->" + modleName + "<- skillID :->" + skillID + "<- \nException: " + e.ToString());
        }
    }

    public static void DestroyFlyObject(int id)
    {
        FlyObjectBase fly = GetFlyObject(id);

        GameObjectManager.DestroyGameObjectByPool(fly.gameObject);

        s_flyList.Remove(fly);
    }

    public static FlyObjectBase GetFlyObject(int id)
    {
        for (int i = 0; i < s_flyList.Count; i++)
        {
            if (s_flyList[i].m_FlyObjID == id)
            {
                return s_flyList[i];
            }
        }

        throw new Exception("GetFlyObject Exception NOT Find " + id);
    }

    public static void CleanFlyObject()
    {
        for (int i = 0; i < s_flyList.Count; i++)
        {
            s_flyList[i].m_isAlive = false;
            GameObjectManager.DestroyGameObjectByPool(s_flyList[i].gameObject);
        }

        s_flyList.Clear();
    }

    #region 接收命令
    
    public static void ReceviceFlyCmd(FlyCmd cmd)
    {
        //if (cmd is SetFlyPosCmd)
        //{
        //    SetFlyPos((SetFlyPosCmd)cmd);
        //}

        if (cmd is CreateFlyObjectCmd)
        {
            CreateFlyObject((CreateFlyObjectCmd)cmd);
        }

        else if (cmd is DestroyFlyObjectCmd)
        {

            DestroyFly((DestroyFlyObjectCmd)cmd);
        }
    }

    public static void CreateFlyObject(CreateFlyObjectCmd cmd)
    {
        FlyObjectManager.CreateFlyObject(cmd.m_flyName, cmd.m_flyID, cmd.m_skillID, cmd.m_createrID, cmd.m_pos, cmd.m_dir);
    }

    //public static void SetFlyPos(SetFlyPosCmd cmd)
    //{
    //    FlyObjectManager.GetFlyObject(cmd.m_flyID).SetPosition(cmd);
    //}

    public static void DestroyFly(DestroyFlyObjectCmd cmd)
    {
        //Debug.Log("DestroyFly " + cmd.m_flyID);

        FlyObjectManager.GetFlyObject(cmd.m_flyID).transform.position = cmd.m_pos;

        if (cmd.m_isShowHitEffect)
        {
            FlyObjectManager.GetFlyObject(cmd.m_flyID).ShowHitEffect();
        }

        FlyObjectManager.DestroyFlyObject(cmd.m_flyID);
    }

    #endregion
}
