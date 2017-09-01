using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class WorldManager
{
    static List<WorldBase> s_worldList = new List<WorldBase>();

    public static List<WorldBase> WorldList { get => s_worldList; set => s_worldList = value; }

    public static WorldBase CreateWorld<T>() where T : WorldBase, new()
    {
        T world = new T();
        world.Init(false);

        s_worldList.Add(world);

        return world;
    }

    public static void DestroyWorld(WorldBase world)
    {
        s_worldList.Remove(world);
    }
}
