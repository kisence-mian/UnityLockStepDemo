using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    static List<WorldBase> s_worldList = new List<WorldBase>();

    public static List<WorldBase> WorldList
    {
        get {
            return s_worldList;
        }

        set {
            s_worldList = value;
        }
    }

    public static int IntervalTime
    {
        get
        {
            return s_intervalTime;
        }

        set
        {
            s_intervalTime = value;
        }
    }

    public static float UpdateSpped
    {
        get
        {
            return s_updateSpped;
        }

        set
        {
            s_updateSpped = value;
            Time.timeScale = s_updateSpped;
        }
    }

    private static float s_updateSpped = 1;

    static int s_intervalTime = 200;
    static float s_UpdateTimer = 0; //ms

    public static void Init(int intervalTime)
    {
        s_intervalTime = intervalTime;
        ApplicationManager.s_OnApplicationUpdate += Update;
        ApplicationManager.s_OnApplicationLateUpdate += LateUpdate;
    }

    public static void Dispose()
    {
        ApplicationManager.s_OnApplicationUpdate -= Update;
        ApplicationManager.s_OnApplicationLateUpdate -= LateUpdate;

        for (int i = 0; i < s_worldList.Count; i++)
        {
            s_worldList[i].Dispose();
        }

        s_worldList.Clear();
    }

    public static WorldBase CreateWorld<T>() where T : WorldBase, new()
    {
        T world = new T();
        world.Init(true);

        s_worldList.Add(world);

        Debug.Log("CreateWorld");
        //GameDataMonitor.PushData("world", world);
      
        return world;
    }

    public static void DestroyWorld(WorldBase world)
    {
        world.Dispose();
        s_worldList.Remove(world);
    }

    static void Update()
    {
        s_UpdateTimer += Time.deltaTime * 1000; //换算成ms

        UpdateWorld((int)(Time.deltaTime * 1000));

        bool isRecalc = false;

        while (s_UpdateTimer > IntervalTime )
        {
            if(!isRecalc)
            {
                isRecalc = true;
                Recalc();
            }

            FixedUpdateWorld(IntervalTime);

            s_UpdateTimer -= IntervalTime;
        }
    }

    static void LateUpdate()
    {
        for (int i = 0; i < s_worldList.Count; i++)
        {
            try
            {
                s_worldList[i].LateUpdate((int)Time.deltaTime * 1000);
            }
            catch (Exception e)
            {
                Debug.LogError("UpdateWorld Exception：" + e.ToString());
            }
        }
    }

    static void FixedUpdateWorld(int deltaTime)
    {
        for (int i = 0; i < s_worldList.Count; i++)
        {
            try
            {
                s_worldList[i].FixedLoop(deltaTime);
            }
            catch (Exception e)
            {
                Debug.LogError("FixedUpdateWorld Exception：" + e.ToString());
            }
        }
    }

    static void UpdateWorld(int deltaTime)
    {
        for (int i = 0; i < s_worldList.Count; i++)
        {
            try
            {
                s_worldList[i].Loop(deltaTime);
            }
            catch (Exception e)
            {
                Debug.LogError("UpdateWorld Exception：" + e.ToString());
            }
        }
    }

    static void Recalc()
    {
        for (int i = 0; i < s_worldList.Count; i++)
        {
            try
            {
                s_worldList[i].CallRecalc();
            }
            catch (Exception e)
            {
                Debug.LogError("UpdateWorld Exception：" + e.ToString());
            }
        }
    }
}
