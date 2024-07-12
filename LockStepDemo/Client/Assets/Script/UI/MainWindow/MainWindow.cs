using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using HDJ.Framework.Tools;

public class MainWindow : UIWindowBase 
{
   
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_ChangeCharacter", OnClickSelectPlayer);
        AddOnClickListener("Button_StartMatch", ReceviceStartMatch);
        AddOnClickListener("Button_Setting", Button_Setting);
        AddOnClickListener("Button_NameChange", Button_NameChange);
        AddOnClickListener("Button_Shop", Button_Shop);

        GlobalEvent.AddTypeEvent<PlayerMatchMsg_c>(ReceviceMatchMsg);
        GlobalEvent.AddTypeEvent<PlayerRename_c>(ReceviceRenameMsg);

        AddEventListener(UserData.UserDataChangeEvent.CharacterChange);
        
        OnRefresh();
    }

  

    public override void OnClose()
    {
        GlobalEvent.RemoveTypeEvent<PlayerMatchMsg_c>(ReceviceMatchMsg);
        GlobalEvent.RemoveTypeEvent<PlayerRename_c>(ReceviceRenameMsg);
    }

    StopwatchTimeData stopwatchTimeData;
    private void ReceviceMatchMsg(PlayerMatchMsg_c e, object[] args)
    {
        if (e.isMatched)
        {
            GameRuntimeData.IsMatchingGame = false;
          
        }
        else
        {
            GameRuntimeData.IsMatchingGame = true;
          string t =  TimeUtils.GetTimeFormat(e.predictTime, "00:00");
            SetText("Text_predict", t);
            stopwatchTimeData = Stopwatch.Add(1, (d) =>
            {
                string t0 = TimeUtils.GetTimeFormat((int)d.TimeCount, "00:00");
                SetText("Text_currentTime", t0);
            });
        }
        SetMatchingUI();
    }

    RenderTexture renderTexture;
    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {
        ReceviceCharacterIDChange();
        SetMatchingUI();
        if (renderTexture == null)
        {
            CreateModelShow();
        }
        else
        {
            if(CharacterID!= UserData.CharacterID)
            {
                Destroy(characterModelRoot);
                CreateModelShow();
            }
        }
    }

    string CharacterID;
    GameObject characterModelRoot;
    void CreateModelShow()
    {
        CharacterID = UserData.CharacterID;
        PlayerDataGenerate data = DataGenerateManager<PlayerDataGenerate>.GetData(UserData.CharacterID);
        GameObject characterModel = UIModelShowTool.Create(data.m_ModelID, out renderTexture);
        MonoBehaviour[] monos = characterModel.GetComponents<MonoBehaviour>();
        for (int i = 0; i < monos.Length; i++)
        {
            monos[i].enabled = false;
        }
        characterModel.transform.localPosition = new Vector3(0, -0.56f, 0);

        RawImage raw = GetRawImage("RawImage_model");

        raw.texture = renderTexture;
        Transform root = characterModel.transform.root;
        characterModelRoot = root.gameObject;
        root.SetParent(raw.transform);
        root.localPosition = Vector3.zero;

        UIModelShowTool.AddDrag(raw.gameObject, characterModel);
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(m_uiRoot, new Vector3(-2000, 0, 0), Vector3.zero, interp: InterpType.OutExpo);

        AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiAlpha(gameObject , null, 0, callBack:(object[] obj) =>
        {
            StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    AnimData m_loopAnim = null;
    void SetMatchingUI()
    {
        if (GameRuntimeData.IsMatchingGame)
        {
            SetActive("MatchUIRoot", true);
            if (m_loopAnim == null)
                m_loopAnim = AnimSystem.Rotate(GetGameObject("Image_Rotation"), Vector3.zero, new Vector3(0, 0, 360), 2f, repeatType: RepeatType.Loop);
        }
        else
        {
            if (m_loopAnim != null)
            {
                AnimSystem.StopAnim(m_loopAnim);
                m_loopAnim = null;
            }
            SetActive("MatchUIRoot", false);
            if (stopwatchTimeData != null)
            {
                stopwatchTimeData.Stop();
                stopwatchTimeData = null;
            }
        }
    }


    #region 事件接收

    void ReceviceStartMatch(InputUIOnClickEvent e)
    {
        if (GameRuntimeData.IsMatchingGame)
        {
            ApplicationStatusManager.GetStatus<MainMenuStatus>().CenelMatch();
            GameRuntimeData.IsMatchingGame = false;
            SetMatchingUI();
        }
        else
        {
            ApplicationStatusManager.GetStatus<MainMenuStatus>().Match();
        }
       
    }

    void OnClickSelectPlayer(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<MainMenuStatus>().SwitchSelectWindow(true);

        //GameStatus.CharacterName = e.m_pram;

        //SetText("Text_CharacterName", "CharacterName ->" + GameStatus.CharacterName);
    }

    void ReceviceCharacterIDChange(params object[] objs)
    {
        //PlayerDataGenerate data = DataGenerateManager<PlayerDataGenerate>.GetData(UserData.CharacterID);

        SetInputText("InputField_Name", UserData.NickName);
       
        SetText("Text_Colin", UserData.CoinNumber.ToString());
        SetText("Text_jewel", UserData.JewelNumber.ToString());
    }
    private void ReceviceRenameMsg(PlayerRename_c e, object[] args)
    {
        if (e.code == ScoketDefine.SUCCESS)
        {
            SetInputText("InputField_Name", e.newName);
        }
    }
    private void Button_NameChange(InputUIOnClickEvent inputEvent)
    {
        UIManager.OpenUIWindow<RenameWindow>();
    }

    private void Button_Setting(InputUIOnClickEvent inputEvent)
    {
        UIManager.OpenUIWindow<SettingWindow>();
    }
    private void Button_Shop(InputUIOnClickEvent inputEvent)
    {
        UIManager.OpenUIWindow<ShopWindow>();
    }
    #endregion
}