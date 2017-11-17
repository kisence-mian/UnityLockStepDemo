using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class GameWorldBase : WorldBase
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
            typeof(SyncSystem<CommandComponent>),
            typeof(ReconnectSystem),

            //Debug
            typeof(SyncDebugSystem),
           };
    }

    //游戏逻辑
    public abstract Type[] GameSystems();
}
