using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.Service
{
    public static class WorldManager
    {
        static List<WorldBase> m_worldList = new List<WorldBase>();

        public static void CreateWorld<T>() where T:WorldBase ,new()
        {
            T world = new T();
            world.Init();

            m_worldList.Add(world);
        }

        public static void DestroyWorld(WorldBase world)
        {
            m_worldList.Remove(world);
        }
    }
}
