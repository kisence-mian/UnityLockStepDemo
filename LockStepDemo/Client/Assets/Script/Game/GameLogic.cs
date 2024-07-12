using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System;

public class GameLogic 
{
    static bool s_isStart = false;
    static bool s_isPause = false;

    public static bool IsPause
    {
        get { return GameLogic.s_isPause; }
        //set { GameLogic.s_isPause = value; }
    }

    private static Camp m_camp = Camp.Lord;

     [NoToLuaAttribute]
    public static Camp myCamp
    {
        get { return GameLogic.m_camp; }
        set { 
            GameLogic.m_camp = value;
            GlobalEvent.DispatchEvent(GameEventEnum.CampChange);
        }
    }

    // [NoToLuaAttribute]
    //public static PlayerInfo myPlayerInfo;

    // [NoToLuaAttribute]
    //public static PlayerControl control;

    // [NoToLuaAttribute]
    //public static List<AIBase> m_AIList = new List<AIBase>();

    // [NoToLuaAttribute]
    //public static FightWindow m_fightWindow = null;

    //[NoToLuaAttribute]
    //public static List<PlayerInfo> playerList = new List<PlayerInfo>();

    [NoToLuaAttribute]
    public static string s_GamePlayModel;

    #region Update

    static void Update()
    {
        //for (int i = 0; i < m_AIList.Count; i++)
        //{
        //    m_AIList[i].Update();
        //}

        //CharacterManager.AreaLogic();
    }

    #endregion
    
    #region 外部调用

    //从这里开始联机和单机的写法一致
    public static void GameStart(string GamePlayModel, string playerListInfo, string playerID,string data)
    {
        //SkyBoxManager.ChangeSkyBox("Sky_07");
        //QualitySettings.shadowDistance = 25;
        //if (GamePlayModel == null || !GamePlayModel.Contains("_"))
        //{
        //    Debug.LogError("无效的GamePlayModel Name! ->" + GamePlayModel + "<-");
        //    return;
        //}

        //Debug.Log("GameStart Data :->" + data+"<-");

        //s_GamePlayModel = GamePlayModel;
        //Dictionary<string, object> content = (Dictionary<string, object>)MiniJSON.Json.Deserialize(data);
        //playerList = AnalysisPlayerInfo(playerListInfo);

        //GameLogicInit(content);
        //PlayerInfoInit(playerList, playerID);

        //GameMemoryService.StartLoading(GamePlayModel, playerList, () =>
        //{
        //    if (SyncService.ServiceType == RouteRule.Local)
        //    {
        //        LoadFinishCallBack();
        //    }
        //    else
        //    {
        //        Dictionary<string, object> msg = HeapObjectPool.GetSODict();
        //        NetworkManager.SendMessage("team2_loadfinish", msg);
        //        HeapObjectPool.PutSODict(msg);
        //    }
        //});
    }

    public static void LoadFinishCallBack()
    {
        s_isStart = true;
        s_isPause = false;
        MapInit(s_GamePlayModel);
        //RegisterEvent();

        //VariableFightUILuaHelper.OpenUI();
    }

    public static void OpenFightWinow(bool isCountDown)
    {
        //if(isCountDown)
        //{
        //    UIManager.OpenUIWindow("FightCountDownWindow", (ui, objs) =>
        //    {
        //        if (m_fightWindow == null)
        //            m_fightWindow = UIManager.OpenUIWindow<FightWindow>();
        //    }, null);
        //}
        //else
        //{
        //    if (m_fightWindow == null)
        //        m_fightWindow = UIManager.OpenUIWindow<FightWindow>();
        //}
    }

    public static void GameWin()
    {
        //Debug.Log("csharp GameWin");

        if (s_isStart)
        {
            s_isStart = false;
            ExitGameLogic();
            GlobalEvent.DispatchEvent(GameEventEnum.GameWin);

            //GameLogicLuaHelper.GameWin(MiniJSON.Json.Serialize(GetGameInfo()));
        }
        else
        {
            Debug.LogError("ExitGameLogic Error 重复调用了游戏结算！");
        }
    }

    public static void GameLose()
    {
        //Debug.Log("csharp GameLose");
        if (s_isStart)
        {
            s_isStart = false;
            ExitGameLogic();
            GlobalEvent.DispatchEvent(GameEventEnum.GameLose);
            //GameLogicLuaHelper.GameLose(MiniJSON.Json.Serialize(GetGameInfo()));
        }
        else
        {
            Debug.LogError("ExitGameLogic Error 重复调用了游戏结算！");
        }
    }

    public static void Dispose()
    {
        ExitGameLogic();
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    /// <param name="isPuase"></param>
    public static void PauseGame(bool isPuase)
    {
        if(SyncService.ServiceType == RouteRule.Local)
        {
            if(s_isPause != isPuase)
            {
                GlobalEvent.DispatchEvent(GameEventEnum.GamePause, IsPause);
                s_isPause = isPuase;

                if (isPuase)
                {
                    UIManager.HideOtherUI("PauseWindow");
                }
                else
                {
                    UIManager.ShowOtherUI("PauseWindow");
                }
            }
        }
    }

   // [NoToLuaAttribute]
   // public static void SetAIParm(int characterID, bool allowMove, bool allowAttack, Vector3? aimPos, int? aimCharacterID ,List<int> weightList = null)
   // {
   //     AIBase ai = GetAI(characterID);

   //     if(ai != null)
   //     {
   //         ai.SetAIParm(allowMove, allowAttack, aimPos, aimCharacterID, weightList);
   //     }
   // }

   //[NoToLuaAttribute]
   // public static void EnterMovieModel()
   // {
   //     if (m_fightWindow != null)
   //         UIManager.HideUI(m_fightWindow);

   //     GlobalEvent.DispatchEvent(GameEventEnum.EnterMovie);
   // }

   // [NoToLuaAttribute]
   // public static void ExitMovieModel()
   // {
   //     if (m_fightWindow != null)
   //         UIManager.ShowUI(m_fightWindow);

   //     GlobalEvent.DispatchEvent(GameEventEnum.ExitMovie);
   // }

    #endregion

    #region ID 管理

    private static int s_MyPlayerID;

    public static int MyPlayerID
    {
        get { return GameLogic.s_MyPlayerID; }
        set { GameLogic.s_MyPlayerID = value; }
    }

    static int s_characterID = 0;
    public static int GetCreateCharacterID()
    {
        return s_characterID++;
    }

    static int s_flyID = 0;

    public static int GetCreateFlyID()
    {
        return s_flyID++;
    }

    public static void ResetID()
    {
        s_flyID = 0;
        s_characterID = 0;
    }

    #endregion

    #region 地图初始化

    static void MapInit(string GamePlayModel)
    {

        //string[] temp = GamePlayModel.Split('_');
        //string playModeName = temp[0];
        //int mapID = int.Parse(temp[1]);

        ////游戏玩法类初始化
        //PlayTypeNameData ptn = PlayTypeDataManager.GetPlayTypeNameData(playModeName, mapID);
        //TirggerRuntimeManager.Instance.Init(ptn.triggerSystemDataName);

        //SceneLoadManager.LoadScence(ptn.mapName);
        //if (s_MyPlayerID != -1)
        //    ControlCharacter(s_MyPlayerID);
        ////});
    }

    #endregion

    #region 流程切换

    [NoToLuaAttribute]
    public static void GameLogicInit(Dictionary<string, object> data)
    {
        AudioManager.Init();
        SyncService.CurrentScene = SyncService.SyncScene.Fight;
        SyncService.Init();
        //CommandRouteService.Init();
        //DropOutService.Init(data);
        //BloodVialManager.Init();

        InputManager.AddListener<InputNetworkMessageEvent>("team2_loadfinish", ReceviceLoadFinish);

        ApplicationManager.s_OnApplicationUpdate += Update;
    }

    //static void PlayerInfoInit( List<PlayerInfo> playerList, string playerID)
    //{
    //    for (int i = 0; i < playerList.Count; i++)
    //    {
    //        if(playerList[i].m_playerID == playerID)
    //        {
    //            s_MyPlayerID = i;
    //            myPlayerInfo = playerList[i];

    //            myCamp = playerList[i].m_camp;

    //            UserPlayerData.Instance.SetWeaponData(myPlayerInfo.m_weaponid);

    //            break;
    //        }
    //    }
    //}

    public static void ExitGameLogic()
    {

            //s_isPause = false;

            //RemoveEvent();

            //m_AIList.Clear();

            //control.Dispose();
            //control = null;

            //s_myPlayer = null;

            //if (m_fightWindow != null)
            //{
            //    UIManager.CloseUIWindow(m_fightWindow, false);
            //    m_fightWindow = null;
            //}

            //VariableFightUILuaHelper.CloseUI();
            //BloodVialManager.Dispose();

            //TirggerRuntimeManager.Instance.Close();
            //DropOutService.Dispose();

            //SyncService.Dispose();
            //CommandRouteService.Dispose();

            //SceneLoadManager.CleanScene();
            //SyncService.ServiceType = RouteRule.Local;

            //CameraService.Instance.Dispose();
            //CharacterManager.CleanCharacter();
            //FlyObjectManager.CleanFlyObject();
            //MaterialManager.ClearnHistory();

            //InputManager.RemoveListener<InputNetworkMessageEvent>("team2_loadfinish", ReceviceLoadFinish);

            ////Timer和Anim要最后卸载
            //Timer.DestroyAllTimer(true);
            //AnimSystem.ClearAllAnim(true);

            //ResetID();

            //MemoryManager.FreeMemory();

            //SkyBoxManager.ChangeSkyColor(new Color(0.16f, 0.16f, 0.16f, 0));
            //ApplicationManager.s_OnApplicationUpdate -= Update;

    }

    //static Dictionary<string, object> GetGameInfo()
    //{
    //    //Dictionary<string, object> GameInfo = new Dictionary<string, object>();
    //    //GameInfo.Add("DropOutInfo", DropOutService.GetNowDropOut());
    //    //GameInfo.Add("MapID", s_GamePlayModel);

    //    return GameInfo;
    //}

    #endregion

    #region 功能函数

    [NoToLuaAttribute]
    static void ControlCharacter(int characterID)
    {
        //if (control != null)
        //{
        //    control.Dispose();
        //}

        //control = new PlayerControl();
        //control.Init();
        //control.m_targetCharacterID = characterID;

        //CameraService.Instance.Init();
        //CameraService.Instance.m_targetID = characterID;
    }

    static CharacterBase s_myPlayer;

    [NoToLuaAttribute]
    //获取主角
    public static CharacterBase GetMyPlayerCharacter()
    {
        if (s_myPlayer == null)
        {
            if (CharacterManager.GetCharacterIsExit(s_MyPlayerID))
            {
                s_myPlayer = CharacterManager.GetCharacter(s_MyPlayerID);
            }
        }

        return s_myPlayer;
    }

    //[NoToLuaAttribute]
    //public static AIBase GetAI(int AIID)
    //{
    //    for (int i = 0; i < m_AIList.Count; i++)
    //    {
    //        if (m_AIList[i].m_controlCharacterID == AIID)
    //        {
    //            return m_AIList[i];
    //        }
    //    }

    //    return null;
    //}


    //[NoToLuaAttribute]
    //public static AIBase GetAIByCharacterType(CharacterTypeEnum characterType)
    //{
    //    if (characterType == CharacterTypeEnum.Brave)
    //    {
    //        return new MonsterAI();
    //    }
    //    else if (characterType == CharacterTypeEnum.Monster)
    //    {
    //        return new MonsterAI();
    //    }
    //    else if (characterType == CharacterTypeEnum.Trap)
    //    {
    //        return new TrapAI();
    //    }
    //    else if (characterType == CharacterTypeEnum.SkillToken)
    //    {
           
    //    }

    //    return null;
    //}

    //[NoToLuaAttribute]
    //public static void AIControlCharacter<AIType>(int characterID) where AIType : AIBase, new()
    //{
    //    AIType control_ai = new AIType();
    //    control_ai.Init(characterID);

    //    m_AIList.Add(control_ai);
    //}

    //[NoToLuaAttribute]
    //public static void AIControlCharacter(int characterID, AIBase control_ai) 
    //{       
    //    control_ai.Init(characterID);

    //    m_AIList.Add(control_ai);
    //}

    static Vector3 m_defaultDir = new Vector3(1, 0, 1);

    [NoToLuaAttribute]
    public static void CreateCharacter(float delayTime, CharacterTypeEnum characterType, string characterName, int characterID, Camp camp, Vector3 pos, Vector3? dir, float amplification)
    {
        CommandRouteService.SendSyncCommand(new CreateCharacterCmd(
            SyncService.CurrentServiceTime + delayTime,
            characterType,
            characterID,
            characterName,
            camp,
            pos,
            dir ?? m_defaultDir,
            amplification
            ));
    }

    [NoToLuaAttribute]
    public static void CreatePlayer(float delayTime, string characterName, int characterID, Camp camp, Vector3 pos, Vector3? dir, float amplification)
    {
        CommandRouteService.SendSyncCommand(new CreateCharacterCmd(
           SyncService.CurrentServiceTime + delayTime,
           CharacterTypeEnum.Brave,
           characterID,
           characterName,
           camp,
           pos,
           dir ?? m_defaultDir,
           amplification
         ));
    }

    [NoToLuaAttribute]
    public static void CreatePlayerAI(float delayTime, string characterName, int characterID, Camp camp, Vector3 pos, Vector3? dir, float amplification)
    {
        CommandRouteService.SendSyncCommand(new CreateCharacterCmd(
           SyncService.CurrentServiceTime + delayTime,
           CharacterTypeEnum.Brave,
           characterID,
           characterName,
           camp,
           pos,
           dir ?? m_defaultDir,
           amplification
         ));

        //AIControlCharacter<MonsterAI>(characterID);
    }

    [NoToLuaAttribute]
    public static void CreateMonster(float delayTime, string characterName, int characterID, Camp camp, Vector3 pos, Vector3? dir, float amplification)
    {
        CommandRouteService.SendSyncCommand(new CreateCharacterCmd(
           SyncService.CurrentServiceTime + delayTime,
           CharacterTypeEnum.Monster,
           characterID,
           characterName,
           camp,
           pos,
           dir ?? m_defaultDir,
           amplification
           ));

        //AIControlCharacter<MonsterAI>(characterID);
    }

    [NoToLuaAttribute]
    public static void CreateTrap(float delayTime, string characterName, int characterID, Camp camp, Vector3 pos, Vector3? dir, float amplification)
    {
        CommandRouteService.SendSyncCommand(new CreateCharacterCmd(
           SyncService.CurrentServiceTime + delayTime,
           CharacterTypeEnum.Trap,
           characterID,
           characterName,
           camp,
           pos,
           dir ?? m_defaultDir,
           amplification
           ));

        //AIControlCharacter<TrapAI>(characterID);
    }

    //static List<PlayerInfo> AnalysisPlayerInfo(string info)
    //{
    //    Debug.Log("AnalysisPlayerInfo: " + info);

    //    //Dictionary<string, object> data = (Dictionary<string, object>);
    //    List<object> list = (List<object>)MiniJSON.Json.Deserialize(info);

    //    playerList.Clear();

    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        Dictionary<string, object> dataTmp = (Dictionary<string, object>)list[i];
    //        PlayerInfo playerInfo = HeapObjectPool<PlayerInfo>.GetObject();

    //        playerInfo.m_playerID      = dataTmp["role_id"].ToString();
    //        playerInfo.m_weaponid      = dataTmp["weapon"].ToString();
    //        playerInfo.characterName   = dataTmp["model_id"].ToString();
    //        //playerInfo.m_camp = (Camp)Enum.Parse(typeof(Camp), dataTmp["camp"].ToString());

    //        playerInfo.m_Hp = int.Parse(dataTmp["hp"].ToString());
    //        playerInfo.m_HpAbsorb = int.Parse(dataTmp["hpabsorb"].ToString());
    //        playerInfo.m_HpRecover = int.Parse(dataTmp["hprecover"].ToString());
    //        playerInfo.m_MoveSpeed = int.Parse(dataTmp["movespeed"].ToString());
    //        playerInfo.m_Attack = int.Parse(dataTmp["att"].ToString());
    //        playerInfo.m_Crit = int.Parse(dataTmp["crit"].ToString());
    //        playerInfo.m_CritDamage = int.Parse(dataTmp["critdamage"].ToString());
    //        playerInfo.m_Defense = int.Parse(dataTmp["def"].ToString());
    //        playerInfo.m_IgnoreDefense = int.Parse(dataTmp["ignoredef"].ToString());
    //        playerInfo.m_Tough = int.Parse(dataTmp["tough"].ToString());

    //        if (dataTmp.ContainsKey("skillList"))
    //        {
    //            List<object> skillList = (List<object>)dataTmp["skillList"];
    //            for (int j = 0; j < skillList.Count; j++)
    //            {
    //                playerInfo.m_skilList.Add(new SkillData(skillList[j].ToString(),j + 1));
    //            }

    //            //加一个采集技能
    //            playerInfo.m_skilList.Add(new SkillData("caiji", 0));
    //        }

    //        if (dataTmp.ContainsKey("attackList"))
    //        {
    //            List<object> attackList = (List<object>)dataTmp["attackList"];
    //            for (int j = 0; j < attackList.Count; j++)
    //            {
    //                playerInfo.m_attackList.Add(new SkillData(attackList[j].ToString()));
    //            }
    //        }

    //        //断言
    //        if (playerInfo.characterName == "")
    //        {
    //            throw new Exception("model_id is Null !");
    //        }

    //        if (playerInfo.m_weaponid == "")
    //        {
    //            throw new Exception("m_weaponid is Null !");
    //        }
            
    //        playerInfo.m_camp        = Camp.Brave;

    //        playerList.Add(playerInfo);
    //    }

    //    if (playerList.Count == 1)
    //    {
    //        SyncService.ServiceType = RouteRule.Local;
    //    }

    //    return playerList;
    //}

    #endregion

    #region 消息接收

    //static void RegisterEvent()
    //{
    //    CharacterManager.AddListener(s_MyPlayerID, CharacterEventType.Die, ReceviceCharacterDie);
    //    //ResurgenceLuaHelper.FinishCallBack += RecevicevResurgence;
    //}

    //static void RemoveEvent()
    //{
    //    CharacterManager.RemoveListener(s_MyPlayerID, CharacterEventType.Die, ReceviceCharacterDie);
    //    //ResurgenceLuaHelper.FinishCallBack -= RecevicevResurgence;
    //}

    [NoToLua]
    static void ReceviceLoadFinish(InputNetworkMessageEvent e)
    {
        LoadFinishCallBack();
    }

    //[NoToLua]
    //static void ReceviceCharacterDie(CharacterEventType eventType, CharacterBase character, params object[] args)
    //{
    //    if (s_isStart && IsOpenResurgenceUI())
    //    {
    //        if (m_fightWindow != null)
    //        {
    //            UIManager.HideUI(m_fightWindow);
    //        }

    //        UIManager.HideOtherUI("FightWindow");

    //        if (SyncService.ServiceType == RouteRule.Local)
    //        {
    //            StopAllAI();
    //        }

    //        ResurgenceLuaHelper.OpenResurgenceUI();
    //    }
    //}

    //static void StopAllAI()
    //{
    //    for (int i = 0; i < m_AIList.Count; i++)
    //    {
    //        m_AIList[i].SetAllowControl(false);
    //    }
    //}

    //static void StartAllAI()
    //{
    //    for (int i = 0; i < m_AIList.Count; i++)
    //    {
    //        m_AIList[i].SetAllowControl(true);
    //    }
    //}

    //[NoToLuaAttribute]
    //static void RecevicevResurgence(ResurgenceStatus status, Dictionary<string, object> data)
    //{
    //    if (status != ResurgenceStatus.Success)
    //    {
    //        GlobalEvent.DispatchEvent(GameEventEnum.NoResurgence, data);
    //    }
    //    else
    //    {
    //        ResurgenceCmd rcmd = HeapObjectPool<ResurgenceCmd>.GetObject();
    //        rcmd.SetData(s_MyPlayerID);
    //        CommandRouteService.SendPursueCommand(rcmd);

    //        GlobalEvent.DispatchEvent(GameEventEnum.Resurgence, data);

    //        UIManager.ShowOtherUI("FightWindow");

    //        if (m_fightWindow != null)
    //            UIManager.ShowUI(m_fightWindow);

    //        if (SyncService.ServiceType == RouteRule.Local)
    //        {
    //            StartAllAI();
    //        }
    //    }
    //}

    //static bool IsOpenResurgenceUI()
    //{
    //    if(SyncService.ServiceType == RouteRule.Local)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        bool allDie = true;
    //        for (int i = 0; i < playerList.Count; i++)
    //        {
    //            if(CharacterManager.GetCharacterIsExit(i))
    //            {
    //                if(CharacterManager.GetCharacter(i).IsAlive)
    //                {
    //                    allDie = false;
    //                }
    //            }
    //        }

    //        return !allDie;
    //    }
    //}

    #endregion
}

public enum GameEventEnum
{
    CampChange,
    GameWin,
    GameLose,
    GamePause,
    NoResurgence,
    Resurgence,
    EnterMovie,
    ExitMovie,
}

public enum CharacterIDEnum
{
    Player,
    Monster,
    Trap,
    SKilToken,
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
