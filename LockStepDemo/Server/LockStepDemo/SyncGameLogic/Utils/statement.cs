using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class statement
{

}

public enum DirectionEnum
{
    Forward, //施法者前方
    Backward,//施法者后方
    Leave,//受击者远离施法者方向
    Close,//受击者靠近施法者方向
}
public enum CharacterEventType
{
    Init,   //初始化
    Move,   //移动
    Attack, //攻击
    Damage, //受伤
    Recover,//恢复
    Die,    //死亡
    SKill,  //使用技能
    BeBreak,//被打断
    Resurgence, //复活
    EnterArea,  //进入某区域
    ExitArea,   //离开某区域
    Destroy,
}

public enum HardPointEnum
{
    head,
    hand_R,
    hand_L,
    chest,
    position, //当前位置，无坐标
    enemy,    //敌人位置处
    Weapon_01,
    Weapon_02,
    headTop, //头上方
}

public enum VisibleEnum
{
    Visible,         //完全可见
    inVisible,       //不可见
    CloakingVisible, //潜行可见
}

public enum Camp
{
    Brave,
    Lord,

    Team1,
    Team2,
    Team3,
    Team4,
    Team5,
    Team6,
    Team7,
    Team8,
    Team9,
    Team10,
    Team11,
    Team12,
    Team13,
    Team14,
    Team15,
    Team16,
    Team17,
    Team18,
    Team19,
    Team20,
}


