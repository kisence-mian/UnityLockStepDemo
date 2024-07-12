using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ComponentName(LogicComponentType.Action, "游戏场景/玩家出生点")]
[Serializable]
public class Action_Player_CreatePositions : ActionComponentBase
{
    public List<Vector3> createPoins = new List<Vector3>();
	
}

