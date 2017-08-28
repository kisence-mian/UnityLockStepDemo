using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class statement
{

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
