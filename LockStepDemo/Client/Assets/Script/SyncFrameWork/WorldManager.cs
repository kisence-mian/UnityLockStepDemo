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

    public static void CreateWorld<T>() where T : WorldBase, new()
    {
        T world = new T();
        world.Init();

        s_worldList.Add(world);
    }

    public static void DestroyWorld(WorldBase world)
    {
        s_worldList.Remove(world);
    }

    static void Update()
    {
        for (int i = 0; i < WorldList.Count; i++)
        {
            try
            {
                //WorldManager.WorldList[i].FixedLoop(deltaTime);
            }
            catch (Exception e)
            {
                Debug.LogError("UpdateWorld Exception：" + e.ToString());
            }
        }
    }
}
