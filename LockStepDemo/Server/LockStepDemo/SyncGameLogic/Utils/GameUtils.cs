using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class GameUtils
{
    public const string c_gameFinish =  "GameFinsih";

    public const int c_element_default = 0;

    public static string GetEventKey(int entityID, CharacterEventType EventType)
    {
        return entityID + EventType.ToString();
    }
}


