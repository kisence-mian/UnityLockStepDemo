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
            //Time.timeScale = s_updateSpped;
        }
    }

    private static float s_updateSpped = 1;

    static int s_intervalTime = 200;
    static float s_UpdateTimer = 0; //ms
    static float s_deltaTime = 0; //ms
    //static int s_logicFrameCount = 1;

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
        world.name = typeof(T).Name;
        world.Init(true);

        s_worldList.Add(world);

        //Debug.Log("CreateWorld");
        //GameDataMonitor.PushData("world", world);
      
        return world;
    }

    public static void DestroyWorld(WorldBase world)
    {
        world.Dispose();
        s_worldList.Remove(world);
    }

    //static bool isUpdate = false;
    static bool isRecalc = false;
    static bool isOptimistic = true;
    static void Update()
    {
        s_UpdateTimer += Time.deltaTime * 1000; //换算成ms
        s_deltaTime += Time.deltaTime;

        UpdateWorld((int)(s_deltaTime * 1000));
        s_deltaTime = 0;

        if(!isOptimistic)
        {
            //重计算过后立即执行
            if (isRecalc)
            {
                FixedUpdateWorld(IntervalTime);
                s_UpdateTimer -= IntervalTime;
                isRecalc = false;
            }
            else
            {
                //进行重计算
                if (s_UpdateTimer > IntervalTime)
                {
                    isRecalc = true;
                    Recalc();
                }
            }
        }
        else
        {
            if(s_UpdateTimer > IntervalTime)
            {
                s_UpdateTimer -= IntervalTime;
                OptimisticFixedUpdateWorld(IntervalTime);

                SpeedControl();
            }
        }
    }

    //取出WorldManager中的第一个进行速度控制
    static void SpeedControl()
    {
        if(s_worldList.Count > 0)
        {
            WorldBase world = s_worldList[0];
            int chacheCount = world.GetCacheCount();

            if (chacheCount <= 2)
            {
                UpdateSpped = 1f;
            }
            else if (chacheCount < 4)
            {
                UpdateSpped = 2f;
            }
            else if (chacheCount < 8)
            {
                UpdateSpped = 4f;
            }
            else
            {
                UpdateSpped = 8f;
                //立即追赶到目标帧
                for (int i = 0; i < chacheCount; i++)
                {
                    world.OptimisticFixedLoop(IntervalTime);
                }
            }
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

    static void OptimisticFixedUpdateWorld(int deltaTime)
    {
        for (int i = 0; i < s_worldList.Count; i++)
        {
            try
            {
                if(s_worldList[i].GetIsAllMsg() || s_worldList[i].IsLocal)
                {
                    s_worldList[i].OptimisticFixedLoop(deltaTime);
                }
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
