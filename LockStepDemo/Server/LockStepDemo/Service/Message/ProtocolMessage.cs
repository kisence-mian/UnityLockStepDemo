using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol;

[Module(10, "ProtocolMessage")]
public abstract class ProtocolMessage : CsharpProtocolInterface
{

}

[MessageMode(SendMode.ToServer)]
public class PlayerLoginMsg_s : ProtocolMessage
{
    public string playerID;
    public string nickName;

}

[MessageMode(SendMode.ToClient)]
public class PlayerLoginMsg_c : ProtocolMessage
{
    public int code;

    public string characterID; //玩家选择的角色

    public List<string> ownCharacter;

    public int coin;
    public int diamond;
}

[MessageMode(SendMode.ToServer)]
public class PlayerMatchMsg_s : ProtocolMessage
{
    public bool isCancel = false;
}

[MessageMode(SendMode.ToClient)]
public class PlayerMatchMsg_c : ProtocolMessage
{
    public int predictTime = 0;     //预计匹配时间
    public bool isMatched = false;   //当匹配成功后会再返回这个消息，并将这个字段置为true
}

[MessageMode(SendMode.ToServer)]
public class PlayerResurgence_s : ProtocolMessage  //玩家复活消息
{

}

[MessageMode(SendMode.ToClient)]
public class PlayerResurgence_c : ProtocolMessage
{

}

[MessageMode(SendMode.ToServer)]
public class PlayerSelectCharacter_s : ProtocolMessage  //玩家选择角色消息
{
    public string characterID;
}

[MessageMode(SendMode.ToClient)]
public class PlayerSelectCharacter_c : ProtocolMessage
{
    public int code;
}

[MessageMode(SendMode.ToClient)]
public class PlayerRename_c : ProtocolMessage
{
    public int code;
    public string newName;
}
[MessageMode(SendMode.ToServer)]
public class PlayerRename_s : ProtocolMessage
{
    public string newName;
}

[MessageMode(SendMode.ToClient)]
public class PlayerSettlement_c : ProtocolMessage
{
    public int rank;
    public int score;
    public int historicalHighest;
    public int diamond;
}

[MessageMode(SendMode.ToClient)]
public class PlayerBuyCharacter_c : ProtocolMessage
{
    public int code;
}
[MessageMode(SendMode.ToServer)]
public class PlayerBuyCharacter_s : ProtocolMessage
{
    public string characterID;
}


