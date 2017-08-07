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

    static int s_intervalTime = 200;
    public static void Init(int intervalTime)
    {
        s_intervalTime = intervalTime;
        ApplicationManager.s_OnApplicationUpdate += Update;
    }

    public static void Dispose()
    {
        ApplicationManager.s_OnApplicationUpdate -= Update;
    }

    public static void CreateWorld<T>() where T : WorldBase, new()
    {
        T world = new T();
        world.Init(true);

        s_worldList.Add(world);
    }

    public static void DestroyWorld(WorldBase world)
    {
        s_worldList.Remove(world);
    }

    static float s_lastUpdateTime = 0;
    static float s_UpdateTimer = 0; //ms
    static void Update()
    {
        s_UpdateTimer += Time.deltaTime * 1000; //换算成ms

        UpdateWorld((int)(Time.deltaTime * 1000));

        while (s_UpdateTimer > s_intervalTime)
        {
            FixedUpdateWorld(s_intervalTime);

            s_UpdateTimer -= s_intervalTime;
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
}
