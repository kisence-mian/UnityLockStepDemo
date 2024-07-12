using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ComponentName(LogicComponentType.Action, "游戏场景/放置物品")]
[Serializable]
public class Action_GameScene_PutObjects : ActionComponentBase
{
    public GameSceneItemType itemType;
    //碎片种类ID
    public List<int> HavePiecesOfElementsId = new List<int>();
    //起始百分比
    public int startHavepPrcent = 5;
    //每次增加百分比
    public int addPeerPrcent;
    //最大百分比
    public int maxHavepPrcent;

}
//物品类型
public enum GameSceneItemType
{
    PiecesOfElements=0,
    RedElixir=1,
}
