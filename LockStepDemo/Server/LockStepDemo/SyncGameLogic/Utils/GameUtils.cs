using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GameUtils
{
    public static string GetEventKey(int entityID, CharacterEventType EventType)
    {
        return entityID + EventType.ToString();
    }
}

