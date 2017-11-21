using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class GameWorldBase<T> : WorldBase where T: PlayerCommandBase,new()
{
    public override Type[] GetSystemTypes()
    {
        Type[] clientSystem = ClientSystem();
        Type[] gameSystems  = GameSystems();

        Type[] types = new Type[clientSystem.Length + gameSystems.Length];

        int index = 0;

        for (int i = 0; i < gameSystems.Length; i++)
        {
            types[index++] = gameSystems[i];
        }

        for (int i = 0; i < clientSystem.Length; i++)
        {
            types[index++] = clientSystem[i];
        }

        return types;
    }

    //客户端同步逻辑
    public Type[] ClientSystem()
    {
        return new Type[]
           {
            typeof(SyncSystem<T>),
            typeof(ReconnectSystem),

            //Debug
            typeof(SyncDebugSystem),
           };
    }

    //游戏逻辑
    public abstract Type[] GameSystems();
}
