using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterManager
{
    static List<CharacterBase> m_characterList = new List<CharacterBase>();

    public static List<CharacterBase> CharacterList
    {
        get { return CharacterManager.m_characterList; }
        set { CharacterManager.m_characterList = value; }
    }

    public static CharacterBase CreateCharacter(CharacterTypeEnum characterType, string characterName, int characterID, Camp camp, Vector3 pos, Vector3 dir, float amplification)
    {
        string modelID = null;
        try
        {
            //加载数据
            //获取模型ID
            CharacterBaseProperty property = GetProperty(characterType, characterName);

            modelID = property.m_modelID;

            GameObject go = GameObjectManager.GetPoolObject(modelID).gameObject;
            CharacterBase character = go.GetComponent<CharacterBase>();

            character.m_characterID = characterID;
            character.transform.position = pos;

            //Debug.Log(" CreateCharacter " + characterID + " "+ pos);

            if (dir != Vector3.zero)
                character.transform.forward = dir;
            character.m_camp = camp;
            character.m_Property = property;
            character.m_Property.m_amplification = amplification;

            if (Application.isPlaying)
            {
                m_characterList.Add(character);
                character.Init(characterName, characterID);
            }
            return character;
        }
        catch(Exception e)
        {
            throw new Exception("CreateCharacter Exception :characterType: " + characterType 
                + " characterName: " + characterName 
                + " characterID: " + characterID
                + " modelID: " + modelID
                + " Camp: " + camp 
                + " pos: " + pos 
                + " dir: " + dir 
                + " amplification: " + amplification 
                + "\n\n" + e.ToString());
        }
    }

    public static CharacterBaseProperty GetProperty(CharacterTypeEnum characterType, string characterName)
    {
        switch (characterType)
        {
            case CharacterTypeEnum.Brave:
                return new BraveProperty(characterName);
            case CharacterTypeEnum.Monster:
                //return new MonsterProperty(characterName);
            case CharacterTypeEnum.Trap:
                //return new TrapProperty(characterName);
            case CharacterTypeEnum.SkillToken:
                return new BraveProperty("SkillToken");
            default:
                throw new Exception("GetProperty Type Error");
        }
    }

    public static void CreateSkillToken(string skillID, int createrID, Camp camp, Vector3 pos, Vector3 dir)
    {
        CharacterBaseProperty property = GetProperty(CharacterTypeEnum.SkillToken, skillID);

        PoolObject go = GameObjectManager.GetPoolObject("SkillToken");
        CharacterBase character = go.GetComponent<CharacterBase>();

        character.m_characterID = createrID;
        character.transform.position = pos;
        character.transform.forward = dir;
        character.m_camp = camp;
        character.m_Property = property;

        character.Init(skillID, createrID);

        SkillCmd scmd = HeapObjectPool<SkillCmd>.GetObject();
        scmd.SetData(0,skillID,dir,pos);
        character.Skill(scmd);
    }

    public static void RemoveSkillToken(SkillToken token)
    {
        GameObjectManager.DestroyGameObjectByPool(token.gameObject);
    }

    public static void DestroyCharacter(int characterID)
    {
        CharacterBase character = GetCharacter(characterID);

        character.Destroy();
        GameObjectManager.DestroyPoolObject(character);
        RemoveByID(characterID);
    }

    public static bool GetCharacterIsExit(int ID)
    {
        for (int i = 0; i < m_characterList.Count; i++)
        {
            if (m_characterList[i].m_characterID == ID)
            {
                return true;
            }
        }

        return false;
    }

    public static CharacterBase GetCharacter(int ID)
    {
        for (int i = 0; i <  m_characterList.Count; i++)
        {
            if(m_characterList[i].m_characterID == ID)
            {
                return m_characterList[i];
            }
        }

        throw new Exception("GetCharacter Exception NOT FIND " + ID);
    }

    static void RemoveByID(int ID)
    {
        for (int i = 0; i < m_characterList.Count; i++)
        {
            if (m_characterList[i].m_characterID == ID)
            {
                m_characterList.RemoveAt(i);
                return;
            }
        }
    }

    public static void CleanCharacter()
    {
        for (int i = 0; i <  m_characterList.Count; i++)
        {
            m_characterList[i].Destroy();

            GameObjectManager.DestroyPoolObject(m_characterList[i]);
        }
        m_characterList.Clear();
    }

    #region 接收命令

    public static void ReceviceCharacterCmd(CharacterCmd cmd)
    {
        if (cmd is CreateCharacterCmd)
        {
            CreateCharacter((CreateCharacterCmd)cmd);
        }

        if (cmd is RemoveCharacterCmd)
        {
            RemoveCharacter((RemoveCharacterCmd)cmd);
        }

        else if (cmd is CreateSkillTokenCmd)
        {
            CreateSkillToken((CreateSkillTokenCmd)cmd);
        }

        else if (cmd is DamageCmd)
        {
            Damage((DamageCmd)cmd);
        }

        else if (cmd is DieCmd)
        {
            Die((DieCmd)cmd);
        }

        else if (cmd is MoveCmd)
        {
            MoveCommand((MoveCmd)cmd);
        }

        else if (cmd is RotationCmd)
        {
            RotationCommand((RotationCmd)cmd);
        }

        else if (cmd is AttackCmd)
        {
            AttackCommand((AttackCmd)cmd);
        }

        else if (cmd is SkillCmd)
        {
            Skill((SkillCmd)cmd);
        }

        else if (cmd is BlowFlyCmd)
        {
            BlowFly((BlowFlyCmd)cmd);
        }

        else if (cmd is AddBuffCmd)
        {
            AddBuff((AddBuffCmd)cmd);
        }

        else if (cmd is RemoveBuffCmd)
        {
            RemoveBuff((RemoveBuffCmd)cmd);
        }

        else if (cmd is RecoverCmd)
        {
            Recover((RecoverCmd)cmd);
        }

        else if (cmd is ResurgenceCmd)
        {
            Resurgence((ResurgenceCmd)cmd);
        }

        else if (cmd is ChangeWeaponCmd)
        {
            ChangeWeapon((ChangeWeaponCmd)cmd);
        }

        //else if (cmd is TrapDamageCmd)
        //{
        //    TrapDamage((TrapDamageCmd)cmd);
        //}
    }

    public static void ReceviceTransmitPursueCmd(CharacterCmd cmd)
    {
        if (cmd is MoveCmd)
        {
            MoveCmd mcmd = (MoveCmd)cmd;

            if (mcmd.CharacterID != GameLogic.MyPlayerID )
            {
                MoveCommand(mcmd);
            }
        }

        else if (cmd is AttackCmd)
        {
            AttackCmd acmd = (AttackCmd)cmd;
            if (acmd.CharacterID != GameLogic.MyPlayerID )
            {
                AttackCommand(acmd);
            }
        }

        else if (cmd is SkillCmd)
        {
            SkillCmd scmd = (SkillCmd)cmd;
            if (scmd.CharacterID != GameLogic.MyPlayerID )
            {
                Skill(scmd);
            }
        }

        else if (cmd is ResurgenceCmd)
        {
            ResurgenceCmd rcmd = (ResurgenceCmd)cmd;
            if (rcmd.CharacterID != GameLogic.MyPlayerID)
            {
                Resurgence(rcmd);
            }
        }
    }


    public static CharacterBase CreateCharacter(CreateCharacterCmd cmd)
    {
        return CharacterManager.CreateCharacter(cmd.m_characterType, 
            cmd.m_characterName,
            cmd.m_characterID,
            cmd.m_camp,
            cmd.m_pos,
            cmd.m_dir,
            cmd.m_amplification);
    }

    public static void RemoveCharacter(RemoveCharacterCmd cmd)
    {
        CharacterBase character =  GetCharacter(cmd.m_characterID);

        m_characterList.Remove(character);
    }

    public static void CreateSkillToken(CreateSkillTokenCmd cmd)
    {
        CreateSkillToken(
            cmd.m_SkillID,
            cmd.m_createrID,
            cmd.m_camp,
            cmd.m_pos,
            cmd.m_dir);
    }

    static void AttackCommand(AttackCmd cmd)
    {
        if (GetCharacterIsExit(cmd.CharacterID))
        {
            GetCharacter(cmd.CharacterID).Attack(cmd);
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    static void MoveCommand(MoveCmd cmd)
    {
        if (GetCharacterIsExit(cmd.CharacterID))
        {
            GetCharacter(cmd.CharacterID).Move(cmd);
        }
    }

    static void RotationCommand(RotationCmd cmd)
    {
        if (GetCharacterIsExit(cmd.CharacterID))
        {
            GetCharacter(cmd.CharacterID).Rotation(cmd);
        }
    }

    static void Skill(SkillCmd cmd)
    {
        if (GetCharacterIsExit(cmd.CharacterID))
        {
            GetCharacter(cmd.CharacterID).Skill(cmd);
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    static void Damage(DamageCmd cmd)
    {
        if (GetCharacterIsExit(cmd.m_characterID))
        {
            GetCharacter(cmd.m_characterID).BeDamage(cmd);
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    static void Die(DieCmd cmd)
    {
        //Debug.Log("recevice Die cmd! " + cmd.CharacterID);

        if (GetCharacterIsExit(cmd.m_characterID))
        {
            CharacterBase l_dieCharacter = GetCharacter(cmd.m_characterID);
            l_dieCharacter.Die(cmd);
            
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    static void Resurgence(ResurgenceCmd cmd)
    {
        if (GetCharacterIsExit(cmd.CharacterID))
        {
            GetCharacter(cmd.CharacterID).Resurgence(cmd);
        }
        else
        {
            Debug.LogError("Resurgence GetCharacter not Exits!");
        }
    }

    static void ChangeWeapon(ChangeWeaponCmd cmd)
    {
        if (GetCharacterIsExit(cmd.CharacterID))
        {
            //Player cb = (Player)GetCharacter(cmd.CharacterID);
            //cb.ChangeWeapon(cmd.m_weaponid);
        }
        else
        {
            Debug.LogError("ChangeWeapon GetCharacter not Exits!");
        }
    }

    static void BlowFly(BlowFlyCmd cmd)
    {
        if (GetCharacterIsExit(cmd.m_flyerID))
        {
            GetCharacter(cmd.m_flyerID).Blowfly(cmd);
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    static void AddBuff(AddBuffCmd cmd)
    {
        if (GetCharacterIsExit(cmd.m_characterID))
        {
            GetCharacter(cmd.m_characterID).AddBuff(cmd);
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    static void RemoveBuff(RemoveBuffCmd cmd)
    {
        if (GetCharacterIsExit(cmd.m_characterID))
        {
            GetCharacter(cmd.m_characterID).RemoveBuff(cmd);
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    static void Recover(RecoverCmd cmd)
    {
        if (GetCharacterIsExit(cmd.m_characterID))
        {
            GetCharacter(cmd.m_characterID).BeRecover(cmd);
        }
        else
        {
            Debug.LogError("GetCharacter not Exits!");
        }
    }

    //static void TrapDamage(TrapDamageCmd cmd)
    //{
    //    if (GetCharacterIsExit(cmd.CharacterID))
    //    {
    //        GetCharacter(cmd.CharacterID).TrapDamage(cmd);
    //    }
    //    else
    //    {
    //        Debug.LogError("GetCharacter not Exits!");
    //    }
    //}

    #endregion

    #region 角色事件

    static Dictionary<string, CharacterEventHandle> m_eventListener = new Dictionary<string, CharacterEventHandle>(); 

    public static void AddListener(int characterID,CharacterEventType EventType,CharacterEventHandle callback)
    {
        string eventKey = characterID + EventType.ToString();

        if(m_eventListener.ContainsKey(eventKey))
        {
            m_eventListener[eventKey] += callback;
        }
        else
        {
            m_eventListener.Add(eventKey,callback);
        }
    }

    public static void AddListener(string eventKey, CharacterEventHandle callback)
    {
        if (m_eventListener.ContainsKey(eventKey))
        {
            m_eventListener[eventKey] += callback;
        }
        else
        {
            m_eventListener.Add(eventKey, callback);
        }
    }

    public static void RemoveListener(int characterID,CharacterEventType EventType,CharacterEventHandle callback)
    {
        string eventKey = characterID + EventType.ToString();

        if (m_eventListener.ContainsKey(eventKey))
        {
            m_eventListener[eventKey] -= callback;
        }
    }

    public static void RemoveListener(string eventKey, CharacterEventHandle callback)
    {
        if (m_eventListener.ContainsKey(eventKey))
        {
            m_eventListener[eventKey] -= callback;
        }
    }

    public static string GetEventKey(int characterID, CharacterEventType EventType)
    {
        return characterID + EventType.ToString();
    }

    public static void Dispatch(int characterID, CharacterEventType EventType, CharacterBase charater,params object[] args)
    {
        string alleventKey = EventType.ToString();
        string eventKey = characterID + alleventKey;
        
        try
        {
            if (m_eventListener.ContainsKey(eventKey) 
                && m_eventListener[eventKey] != null)
            {
                m_eventListener[eventKey](EventType, charater, args);
            }

            if (m_eventListener.ContainsKey(alleventKey) 
                && m_eventListener[alleventKey] != null)
            {
                m_eventListener[alleventKey](EventType, charater, args);
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    #endregion

    //#region 角色区域事件

    //static List<AreaEventData> m_areaLisener = new List<AreaEventData>();

    //public static void AddAreaListener(Area area,int areaID)
    //{
    //    AreaEventData tmp = new AreaEventData();
    //    tmp.m_area = area;
    //    tmp.m_areaID = areaID;

    //    m_areaLisener.Add(tmp);
    //}

    //public static void RemoveAreaListener(int areaID)
    //{
    //    for (int i = 0; i < m_areaLisener.Count; i++)
    //    {
    //        if (m_areaLisener[i].m_areaID == areaID)
    //        {
    //            m_areaLisener.RemoveAt(i);
    //        }
    //    }
    //}

    //public static void AreaLogic()
    //{
    //    for (int i = 0; i < m_areaLisener.Count; i++)
    //    {
    //         //List<CharacterBase> result = FightLogicService.GetAreaListNoCamp(m_areaLisener[i].m_area);
    //         //Judge(m_areaLisener[i], result);
    //    }
    //}

    //public static void Judge(AreaEventData data, List<CharacterBase> result)
    //{
    //    for (int i = 0; i < result.Count; i++)
    //    {
    //        if(!data.m_playerList.Contains(result[i]))
    //        {
    //            //Debug.Log("进入");
    //            Dispatch(result[i].m_characterID, CharacterEventType.EnterArea, result[i], data.m_areaID);
    //            data.m_playerList.Add(result[i]);
    //        }
    //    }

    //    for (int i = 0; i < data.m_playerList.Count; i++)
    //    {
    //        if (!result.Contains(data.m_playerList[i]))
    //        {
    //            //Debug.Log("退出");
    //            Dispatch(data.m_playerList[i].m_characterID, CharacterEventType.ExitArea, data.m_playerList[i], data.m_areaID);
    //            data.m_playerList.Remove(data.m_playerList[i]);
    //        }
    //    }
    //}



    //#endregion

}

public delegate void CharacterEventHandle(CharacterEventType eventType,CharacterBase character,params object[] args);

public class AreaEventData
{
    public int m_areaID;
    public Area m_area;
    public List<CharacterBase> m_playerList = new List<CharacterBase>();
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
