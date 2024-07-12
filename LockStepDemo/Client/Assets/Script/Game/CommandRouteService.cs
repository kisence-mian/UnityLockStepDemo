using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Protocol;

public class CommandRouteService
{
    public static void Init()
    {
        ApplicationManager.s_OnApplicationUpdate += Update;
    }

    public static void Dispose()
    {
        ApplicationManager.s_OnApplicationUpdate -= Update;

        s_syncList.Clear();
    }

    #region Update

    static List<SyncCommandBase> s_syncList = new List<SyncCommandBase>();
    static void Update()
    {
        for (int i = 0; i <s_syncList.Count; i++)
		{
            if (s_syncList[i].m_executeTime < SyncService.CurrentServiceTime)
            {
                try
                {
                    ExecuteSyncCmd(s_syncList[i]);
                }
                catch(Exception e)
                {
                    Debug.LogError(e.ToString());
                }

                if (s_syncList.Count > i)
                {
                    s_syncList.RemoveAt(i);
                    i--;
                }
            }
		}
    }

    #endregion

    #region 发送指令

    public static void SendSyncCommand(SyncCommandBase cmd)
    {
        ProtocolAnalysisService.SendCommand(cmd);

        ExecuteSyncCmd(cmd);
    }
    
    public static void SendPursueCommand(PursueCommandBase cmd)
    {
        ProtocolAnalysisService.SendCommand(cmd);

        //ExecutePursueCmd(cmd);
    }

    public static void SendPursueCommandAI(PursueCommandBase cmd)
    {
        if (SyncService.ServiceType != RouteRule.Local)
        {
            SendTransmitPursueCommand(cmd);
            //ExecutePursueCmd(cmd);
        }
        else if (SyncService.ServiceType == RouteRule.Local)
        {
            ExecutePursueCmd(cmd);
        }
    }

    public static void SendTransmitPursueCommand(PursueCommandBase cmd)
    {
        //ProtocolAnalysisService.SendCommand(cmd);
    }

    #endregion

    #region 接收指令

    public static void ReceviceSyncCommand(SyncCommandBase cmd)
    {
        //同步命令等到执行时机去执行
        if (SyncService.ServiceType != RouteRule.Local)
        {
            s_syncList.Add(cmd);
        }
    }

    public static void RecevicePursueCommand(PursueCommandBase cmd)
    {
        ExecutePursueCmd(cmd);
    }

    public static void ReceviceTransmitPursueCommand(PursueCommandBase cmd)
    {
        //执行
        ExecuteTransmitPursueCmd(cmd);

        if (cmd.CharacterID == GameLogic.MyPlayerID)
        {
            //更新延迟
            float clientTime = cmd.GetCreateTime();
            SyncService.UpdatePing(clientTime);
        }
    }


    /// <summary>
    /// 接收同步命令
    /// </summary>
    /// <param name="e"></param>
    static void ReceviceSyncCommand(InputNetworkMessageEvent e)
    {
        SyncCommandBase cmd = (SyncCommandBase)JsonUtility.FromJson(e.Data["content"].ToString(), Type.GetType(e.Data["type"].ToString()));

        //同步命令等到执行时机去执行
        if (SyncService.ServiceType != RouteRule.Local)
        {
            s_syncList.Add(cmd);
        }
    }

    /// <summary>
    /// 接收追赶命令
    /// </summary>
    static void RecevicePursueCommand(InputNetworkMessageEvent e)
    {
        //PursueCommandBase cmd = (PursueCommandBase)JsonUtility.FromJson(e.Data["content"].ToString(), Type.GetType(e.Data["type"].ToString()));

        //如果是主机，则验证转发并执行
        if (SyncService.ServiceType == RouteRule.Service)
        {
            //验证时间
            float clientTime = float.Parse(e.Data["time"].ToString());
            SyncService.ReceviceClientTime(clientTime);
        }
    }

    /// <summary>
    /// 接收转发命令
    /// </summary>
    static void ReceviceTransmitPursueCommand(InputNetworkMessageEvent e)
    {
        PursueCommandBase cmd = (PursueCommandBase)JsonUtility.FromJson(e.Data["content"].ToString(), Type.GetType(e.Data["type"].ToString()));
            //执行
        ExecuteTransmitPursueCmd(cmd);

        if (cmd.CharacterID == GameLogic.MyPlayerID)
        {
            //更新延迟
            float clientTime = float.Parse(e.Data["time"].ToString());
            SyncService.UpdatePing(clientTime);

            //Debug.Log(e.Data["type"].ToString() + "\n"+e.Data["time"].ToString() + "\n ReceviceTime: " + clientTime + " \nServiceTime:" + SyncService.CurrentServiceTime + "\noffset :" + (SyncService.CurrentServiceTime * 1000 - clientTime));
        }
    }
    #endregion

    #region 执行指令

    /// <summary>
    /// 执行追赶指令
    /// </summary>
    /// <param name="cmd"></param>
    static void ExecutePursueCmd(PursueCommandBase cmd)
    {
        if (cmd is CharacterCmd)
        {
            CharacterManager.ReceviceCharacterCmd((CharacterCmd)cmd);
        }

        else if (cmd is FlyCmd)
        {
            FlyObjectManager.ReceviceFlyCmd((FlyCmd)cmd);
        }

        else if (cmd is ItemCmd)
        {
            ItemManager.ReceviceItemCmd((ItemCmd)cmd);
        }
    }

    /// <summary>
    /// 执行转发追赶指令
    /// </summary>
    /// <param name="cmd"></param>
    static void ExecuteTransmitPursueCmd(PursueCommandBase cmd)
    {
        if (cmd is CharacterCmd)
        {
            CharacterManager.ReceviceTransmitPursueCmd((CharacterCmd)cmd);
        }
    }

    /// <summary>
    /// 执行同步指令
    /// </summary>
    /// <param name="cmd"></param>
    static void ExecuteSyncCmd(SyncCommandBase cmd)
    {
        //Debug.Log("当前时间 " + SyncService.CurrentServiceTime + " 命令时间 " + cmd.m_executeTime + " 差值 " + (SyncService.CurrentServiceTime - cmd.m_executeTime));

        if(cmd is CharacterCmd)
        {
            CharacterManager.ReceviceCharacterCmd((CharacterCmd)cmd);
        }

        else if (cmd is FlyCmd)
        {
            FlyObjectManager.ReceviceFlyCmd((FlyCmd)cmd);
        }

        else if (cmd is ItemCmd)
        {
            ItemManager.ReceviceItemCmd((ItemCmd)cmd);
        }
    }

    /// <summary>
    /// 主机执行追赶命令
    /// </summary>
    /// <param name="cmd"></param>
    static PursueCommandBase ExecutePursueCmdByService(PursueCommandBase cmd)
    {
        if (cmd is CharacterCmd)
        {
            CharacterManager.ReceviceCharacterCmd((CharacterCmd)cmd);
        }

        else if (cmd is FlyCmd)
        {
            FlyObjectManager.ReceviceFlyCmd((FlyCmd)cmd);
        }


        return cmd;
    }

    #endregion

    #region 指令派发

    static Dictionary<int, CharacterCmdCallBack> s_characterCallDict = new Dictionary<int, CharacterCmdCallBack>();
    static Dictionary<int, FlyCmdCallBack> s_flyCallBackDict = new Dictionary<int, FlyCmdCallBack>();

    public static void AddCharacterCmdListener(int characterID,CharacterCmdCallBack callBack)
    {
        if (s_characterCallDict.ContainsKey(characterID))
        {
            s_characterCallDict[characterID] += callBack;
        }
        else
        {
            s_characterCallDict.Add(characterID, callBack);
        }
    }

    public static void RemoveCharacterCmdListener(int characterID, CharacterCmdCallBack callBack)
    {
        if (s_characterCallDict.ContainsKey(characterID))
        {
            s_characterCallDict[characterID] -= callBack;
        }
        else
        {
        }
    }

    public static void AddFlyCmdListener(int flyID,FlyCmdCallBack callBack)
    {
        if (s_flyCallBackDict.ContainsKey(flyID))
        {
            s_flyCallBackDict[flyID] += callBack;
        }
        else
        {
            s_flyCallBackDict.Add(flyID, callBack);
        }
    }

    public static void RemoveFlyCmdListener(int flyID, FlyCmdCallBack callBack)
    {
        if (s_flyCallBackDict.ContainsKey(flyID))
        {
            s_flyCallBackDict[flyID] -= callBack;
        }
        else
        {
        }
    }

    #endregion
}

    #region 指令定义

    #region 基类

    [Module(43,"fight")]
    public abstract class CommandBase:CsharpProtocolInterface
    {
    }


    /// <summary>
    /// 追赶指令基类
    /// </summary>
    public abstract class PursueCommandBase : CommandBase
    {
        /// <summary>
        /// 指令创建时间
        /// </summary>
        [Int16]
        public int m_creatComandTime = 0;

        public int m_characterID;

        public PursueCommandBase()
        {
            Reset();
        }

        public void Reset()
        {
            m_creatComandTime = (int)(SyncService.CurrentServiceTime * 10);
        }

        public int CharacterID
        {
            get
            {
                return m_characterID;
            }

            set
            {
                m_characterID = value;
            }
        }

        public float GetCreateTime()
        {
            return m_creatComandTime / 10f;
        }

        public void SetCreateTime(float time)
        {
            m_creatComandTime = (int)(time * 10);
        }
    }

    /// <summary>
    /// 同步指令基类
    /// </summary>
    public abstract class SyncCommandBase : CommandBase
    {
        /// <summary>
        /// 指令执行时间
        /// </summary>
        public float m_executeTime;

        public SyncCommandBase(float executeTime)
        {
            m_executeTime = executeTime;
        }
    }

    public interface CharacterCmd{ };
    public interface FlyCmd { }
    public interface TrapCmd { };
    public interface BloodVialCmd { };
    
    public interface ItemCmd { };

#endregion

    #region 飞行物相关

public class CreateFlyObjectCmd : SyncCommandBase,FlyCmd
    {
        public string m_flyName;
        public int m_flyID;
        public string m_skillID;
        public int m_createrID;
        public Vector3 m_pos;
        public Vector3 m_dir;

        public CreateFlyObjectCmd()
            : base(0)
        {

        }

        public void SetData(float executeTime, int flyID, string skillID, int createrID, string flyName, Vector3 pos, Vector3 dir)
        {
            m_executeTime = executeTime;
            m_flyID = flyID;
            m_skillID = skillID;
            m_createrID = createrID;
            m_flyName = flyName;
            m_pos = pos;
            m_dir = dir;
        }

        public CreateFlyObjectCmd(float executeTime,int flyID,string skillID,int createrID,string flyName,Vector3 pos,Vector3 dir) 
            :base(executeTime)
        {
            m_flyID = flyID;
            m_skillID = skillID;
            m_createrID = createrID;
            m_flyName = flyName;
            m_pos = pos;
            m_dir = dir;
 
        }
    }

    public class DestroyFlyObjectCmd : SyncCommandBase, FlyCmd
    {
        public int m_flyID;
        public bool m_isShowHitEffect;
        public Vector3 m_pos;

        public DestroyFlyObjectCmd()
            : base(0)
        {

        }

        public void SetData(float executeTime, int flyID, bool isShowHitEffect, Vector3 pos)
        {
            m_executeTime = executeTime;
            m_flyID = flyID;
            m_isShowHitEffect = isShowHitEffect;
            m_pos = pos;
        }

        public DestroyFlyObjectCmd(float executeTime,int flyID,bool isShowHitEffect,Vector3 pos) 
            :base(executeTime)
        {
            m_flyID = flyID;
            m_isShowHitEffect = isShowHitEffect;
            m_pos = pos;
        }
    }

    #endregion

    #region 角色相关

    #region 同步指令
    public class CreateCharacterCmd : SyncCommandBase, CharacterCmd
    {
        public CharacterTypeEnum m_characterType;
        public string m_characterName;
        public int m_characterID;
        public Camp m_camp;

        public Vector3 m_pos;
        public Vector3 m_dir;

        public float m_amplification;

        public CreateCharacterCmd()
            : base(0)
        {
        }

        public CreateCharacterCmd(float executeTime, CharacterTypeEnum characterType, int characterID, string characterName, Camp camp, Vector3 pos, Vector3 dir, float amplification)
            : base(executeTime)
        {
            m_characterType = characterType;
            m_characterID   = characterID;
            m_characterName = characterName;

            m_camp = camp;

            m_pos = pos;
            m_dir = dir;

            m_amplification = amplification;
        }
    }

public class RemoveCharacterCmd : SyncCommandBase, CharacterCmd
{
    public int m_characterID;

    public RemoveCharacterCmd(float executeTime) : base(executeTime)
    {
    }

    public RemoveCharacterCmd() : base(0)
    {

    }
}

public class CreateSkillTokenCmd : SyncCommandBase, CharacterCmd
    {
        //public CharacterTypeEnum m_characterType;
        public string m_SkillID;
        public int m_createrID;
        public Camp m_camp;

        public Vector3 m_pos;
        public Vector3 m_dir;

        public CreateSkillTokenCmd()
            : base(0)
        {

        }

        public void SetData(float executeTime, int createrID, string skillID, Camp camp, Vector3 pos, Vector3 dir)
        {
            m_executeTime = executeTime;
            m_createrID = createrID;
            m_SkillID = skillID;

            m_camp = camp;

            m_pos = pos;
            m_dir = dir;
        }
    }

    public class DieCmd : SyncCommandBase, CharacterCmd
    {
        public int m_characterID;
        public int m_killerID;
        //public Vector3 m_pos;

        public DieCmd()
            : base(0)
        {

        }

        public void SetData(float executeTime, int characterID, int killerID)
        {
            m_executeTime = executeTime;
            m_characterID = characterID;
            m_killerID = killerID;
        }

        public DieCmd(float executeTime, int characterID,int killerID,Vector3 pos)
            : base(executeTime)
        {
            m_characterID = characterID;
            m_killerID = killerID;
            //m_pos = pos;
        }
    }

    public class DamageCmd : SyncCommandBase, CharacterCmd
    {
        public int m_characterID;
        public int m_attackerID;
        public int m_damageNumber = 0;
        public string m_skillID;
        public string m_buffID;

        public bool m_Crit;       //暴击
        public bool m_Disrupting; //破防

        public DamageCmd()
            : base(0)
        {

        }

        public DamageCmd(float executeTime, int characterID, int attackerID, string skillID,string buffID ,int damageNumber )
            : base(executeTime)
        {
            m_characterID = characterID;
            m_attackerID = attackerID;
            m_damageNumber = damageNumber;
            m_skillID = skillID;
            m_buffID = buffID;
        }

        public void SetData(float executeTime, int characterID, int attackerID, string skillID, string buffID, int damageNumber, bool crit, bool disrupting)
        {
            m_executeTime = executeTime;
            m_characterID = characterID;
            m_attackerID = attackerID;
            m_damageNumber = damageNumber;
            m_skillID = skillID;
            m_buffID = buffID;
            m_Crit = crit;
            m_Disrupting = disrupting;
        }
    }


    public class RecoverCmd : SyncCommandBase, CharacterCmd
    {
        public int m_characterID;
        public int m_attackerID;
        public int m_recoverNumber;
        public bool m_isAutoRecover;
        public string m_skillID;
        public string m_buffID;

        public RecoverCmd()
            : base(0)
        {

        }

        public RecoverCmd(float executeTime, int characterID, int attackerID, string skillID, string buffID, int recoverNumber)
            : base(executeTime)
        {
            m_characterID = characterID;
            m_attackerID = attackerID;
            m_recoverNumber = recoverNumber;
            m_skillID = skillID;
            m_buffID = buffID;
        }

        public void SetData(float executeTime, int characterID, int attackerID, string skillID, string buffID, int recoverNumber,bool isAutoRecover)
        {
            m_executeTime = executeTime;
            m_characterID = characterID;
            m_attackerID = attackerID;
            m_recoverNumber = recoverNumber;
            m_skillID = skillID;
            m_buffID = buffID;
            m_isAutoRecover = isAutoRecover;
        }
    }

    public class AddBuffCmd : SyncCommandBase, CharacterCmd
    {
        public int m_characterID;
        public int m_attackerID;
        //public int m_damageNumber = 0;
        public string m_buffID;
        public string m_skillID;

        public AddBuffCmd()
            : base(0)
        { }

        public AddBuffCmd(float executeTime, int characterID, int attackerID, string buffID,string skillID)
            : base(executeTime)
        {
            SetData(executeTime, characterID, attackerID, buffID, skillID);
        }

        public void SetData(float executeTime, int characterID, int attackerID, string buffID, string skillID)
        {
            m_executeTime = executeTime;
            m_characterID = characterID;
            m_attackerID = attackerID;
            m_buffID = buffID;
            m_skillID = skillID;
        }
    }

    public class RemoveBuffCmd : SyncCommandBase, CharacterCmd
    {
        public int m_characterID;
        public string m_buffID;

        public RemoveBuffCmd()
            : base(0)
        { }

        public RemoveBuffCmd(float executeTime, int characterID, string buffID)
            : base(executeTime)
        {
            SetData(executeTime, characterID, buffID);
        }

        public void SetData(float executeTime, int characterID, string buffID)
        {
            m_executeTime = executeTime;
            m_characterID = characterID;
            m_buffID = buffID;
        }
    }

    public class BlowFlyCmd : SyncCommandBase, CharacterCmd
    {
        public int m_flyerID;
        public int m_attackerID;
        public string m_shiftID;

        public Vector3 m_attackerPos;
        public Vector3 m_hurterPos;

        public BlowFlyCmd()
            : base(0)
        {

        }

        public void SetData(float executeTime, int flyerID, int attackerID, string shiftID, Vector3 attackerPos, Vector3 pos)
        {
            m_executeTime = executeTime;
            m_flyerID = flyerID;
            m_attackerID = attackerID;
            m_shiftID = shiftID;

            m_attackerPos = attackerPos;
            m_hurterPos = pos;
        }

        public BlowFlyCmd(float executeTime, int flyerID, int attackerID, string shiftID,Vector3 attackerPos,Vector3 pos)
            : base(executeTime)
        {
            m_flyerID = flyerID;
            m_attackerID = attackerID;
            m_shiftID = shiftID;

            m_attackerPos = attackerPos;
            m_hurterPos = pos;
        }
    }

    #endregion

    #region 追赶指令

    public class ChangeWeaponCmd : PursueCommandBase, CharacterCmd
    {
        public string m_weaponid;
        public void SetData(int characterID, string weaponid)
        {
            CharacterID = characterID;
            m_weaponid = weaponid;
        }
    }

    /// <summary>
    /// 复活命令
    /// </summary>
    public class ResurgenceCmd : PursueCommandBase, CharacterCmd
    {
        public Vector3 m_pos;
        public void SetData(int characterID)
        {
        CharacterID = characterID;
        }
    }

    public class AttackCmd : PursueCommandBase, CharacterCmd
    {
        public Vector3 m_dir;
        public Vector3 m_pos;

        public AttackCmd()
        {

        }

        public void SetData(int characterID,Vector3 attackDir,Vector3 pos)
        {
            Reset();
        CharacterID = characterID;
            m_dir = attackDir;
            m_pos = pos;
        }
    }

    public class MoveCmd : PursueCommandBase, CharacterCmd
    {
        public float m_dirx;
        public float m_dirz;
        public Vector3 m_pos;
        public bool m_isOnlyTurn;

        public MoveCmd()
        {

        }

        public void SetData(int characterID, Vector3 dir, bool isOnlyTurn = false)
        {
            Reset();
            CharacterID = characterID;
            SetDir(dir);
            m_isOnlyTurn = isOnlyTurn;
        }

        public void SetDir(Vector3 dir)
        {
            m_dirx = dir.x;
            m_dirz = dir.z;
        }

        public Vector3 GetDir()
        {
            return new Vector3(m_dirx, 0, m_dirz);
        }
}

    public class SkillCmd : PursueCommandBase, CharacterCmd
    {
        public string m_skillID;
        public Vector3 m_pos;
        public Vector3 m_skillDir;
        
        public SkillCmd()
        {

        }

        public void SetData(int characterID, string skillID, Vector3 skillDir, Vector3 pos)
        {
            Reset();
            CharacterID = characterID;
            //m_status = status;
            m_skillID = skillID;
            m_skillDir = skillDir;
            m_pos = pos;
        }
    }

public class RotationCmd : PursueCommandBase, CharacterCmd
{
    public Vector3 m_dir;

    public RotationCmd()
    {

    }

    public void SetData(int characterID, Vector3 dir)
    {
        Reset();
        CharacterID = characterID;
        m_dir = dir;
    }
}

#endregion

#endregion

    #region 陷阱相关

    #region 同步命令

public class CreateTrapCmd:SyncCommandBase,TrapCmd
    {
        public int m_trapID;
        public string m_trapName;
        public Vector3 m_pos;
        public Vector3 m_dir;
        public Camp m_camp;

        public CreateTrapCmd()
            : base(0)
        {
        }

        public CreateTrapCmd(float executeTime,int trapID, string trapName, Camp camp,Vector3 pos, Vector3 dir)
            : base(executeTime)
        {
            m_trapID = trapID;
            m_trapName = trapName;
            m_pos = pos;
            m_dir = dir;
            m_camp = camp;
        }
    }

    #endregion

    #region 追赶命令

    public class TrapTriggerCmd : SyncCommandBase, TrapCmd
    {
        public int m_trapID;

        public TrapTriggerCmd()
        : base(0)
        {
        }

        public TrapTriggerCmd(float executeTime, int trapID)
                : base(executeTime)
            {
                m_trapID = trapID;
            }
    }

    #endregion

    #endregion

    #region 血瓶相关

    public class CreateBloodVialCmd : SyncCommandBase, BloodVialCmd
    {
        public int m_bloodVialID;
        public Camp m_camp;

        public Vector3 m_pos;

        public CreateBloodVialCmd()
            : base(0)
        {
        }

        public CreateBloodVialCmd(float executeTime, Vector3 pos ,Camp camp)
            : base(executeTime)
        {
            m_camp = camp;

            m_pos = pos;
        }

        public void SetData(float executeTime, Vector3 pos,int bloodVialID ,Camp camp)
        {
            m_executeTime = executeTime;
            m_bloodVialID = bloodVialID;
            m_camp = camp;

            m_pos = pos;
        }
    }

    public class PickUpBloodVialCmd : SyncCommandBase, BloodVialCmd
    {
        public int m_bloodVialID;
        public int m_characterID;

        public PickUpBloodVialCmd()
            : base(0)
        {
        }

        public PickUpBloodVialCmd(float executeTime, int bloodVialID, int characterID)
            : base(executeTime)
        {

            m_bloodVialID = bloodVialID;
            m_characterID = characterID;
        }

        public void SetData(float executeTime, int bloodVialID, int characterID)
        {
            m_executeTime = executeTime;
            m_bloodVialID = bloodVialID;
            m_characterID = characterID;
        }
    }

    public class DestroyBloodVialCmd : SyncCommandBase, BloodVialCmd
    {
        public int m_bloodVialID;

        public DestroyBloodVialCmd()
            : base(0)
        {
        }

        public DestroyBloodVialCmd(float executeTime, int bloodVialID)
            : base(executeTime)
        {

            m_bloodVialID = bloodVialID;
        }

        public void SetData(float executeTime, int bloodVialID)
        {
            m_executeTime = executeTime;
            m_bloodVialID = bloodVialID;
        }
    }



#endregion

    #region 物品相关

//public class CreateItemCmd : SyncCommandBase, ItemCmd
//{
//    public int m_ItemID;
//    public string m_ItemName;
//    //public Camp m_camp;

//    public Vector3 m_pos;

//    List<p_map_item> list;

//    public CreateItemCmd()
//        : base(0)
//    {
//    }

//    public CreateItemCmd(float executeTime, Vector3 pos)
//        : base(executeTime)
//    {
//        //m_camp = camp;

//        m_pos = pos;
//    }

//    public void SetData(float executeTime, Vector3 pos, int ItemlID,string ItemName)
//    {
//        m_executeTime = executeTime;
//        m_ItemID = ItemlID;
//        m_ItemName = ItemName;

//        m_pos = pos;
//    }
//}

public class PickUpItemCmd : SyncCommandBase, ItemCmd
{
    public int m_ItemID;
    public int m_characterID;
    //public int m_ItemNum;

    public PickUpItemCmd()
        : base(0)
    {
    }

    public PickUpItemCmd(float executeTime, int ItemlID, int characterID)
        : base(executeTime)
    {

        m_ItemID = ItemlID;
        m_characterID = characterID;
    }

    public void SetData(float executeTime, int ItemlID, int characterID)
    {
        m_executeTime = executeTime;
        m_ItemID = ItemlID;
        m_characterID = characterID;
    }
}

public class DestroyItemCmd : SyncCommandBase, ItemCmd
{
    public int m_ItemID;

    public DestroyItemCmd()
        : base(0)
    {
    }

    public DestroyItemCmd(float executeTime, int ItemlID)
        : base(executeTime)
    {

        m_ItemID = ItemlID;
    }

    public void SetData(float executeTime, int ItemlID)
    {
        m_executeTime = executeTime;
        m_ItemID = ItemlID;
    }
}


#endregion

#endregion

public delegate void CharacterCmdCallBack(CharacterCmd cmd);
public delegate void FlyCmdCallBack(CharacterCmd cmd);

    public enum RouteRule
    {
        Local,
        Service,
        Client
    }