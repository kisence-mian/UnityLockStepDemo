using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Protocol.fightModule;
using Protocol;

public class OperationWindow : UIWindowBase 
{
    const string c_itemName = "OperationWindown_RankItem";

    const string c_elementItem = "OperationWindown_ElementItem";

    const string c_propItem = "OperationWindow_ItemItem";

    public EntityBase m_entity;

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        InitJoyStick();

        //InitRank();

        //ElementInit();

        //ItemInit();
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {
    }

    public override void OnClose()
    {
        DisposeJoySitck();

        DisposeRank();

        ElementDispose();

        ItemDispose();
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        //目前进入动画不要修改颜色，会跟设置元素可用状态冲突

        return base.EnterAnim(l_animComplete, l_callBack, objs);
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        return base.ExitAnim(l_animComplete, l_callBack, objs);
    }

    #region Update
    private void Update()
    {
        //return;
        ShowPingText();
        ShowLeftTime();
        //1逻辑
        AdsorbLogic("Image_BG_1", 0);

        //2逻辑
        AdsorbLogic("Image_BG_2", 1);

    }

    private void ShowPingText()
    {
      int ping =  m_entity.World.GetSingletonComp<ConnectStatusComponent>().rtt / 2;
        string temp = "";
        if (ping <= 80)
        {
            temp += "<color=green>Ping:" + ping + "</color>";
        }
        else if (ping > 80 && ping <= 180)
        {
            temp += "<color=#ffa500ff>Ping:" + ping + "</color>";
        }
        else 
        {
            temp += "<color=red>Ping:" + ping + "</color>";
        }
        SetText("Text_Ping", temp);
    }

    private void ShowLeftTime()
    {
        int time = m_entity.World.GetSingletonComp<GameTimeComponent>().GameTime/1000;
       string ss = TimeUtils.GetTimeFormat(time, "00:00");
        SetText("Text_time", ss);
    }
    #endregion

    #region 排行榜

    List<UIBase> m_rankList = new List<UIBase>();

    public void InitRank()
    {
        m_entity.World.eventSystem.AddListener(GameUtils.c_scoreChange, UpdateRank);
        UpdateRank(null);
    }

    void DisposeRank()
    {
        m_entity.World.eventSystem.RemoveListener(GameUtils.c_scoreChange, UpdateRank);
    }
    void UpdateRank(EntityBase entity, params object[] pbjs)
    {
        //Debug.Log("Recevice Rank " + GameData.RankLists.Count);

       string myID =  m_entity.GetComp<PlayerComponent>().characterID;

        List<PlayerComponent> rankList = m_entity.World.GetSingletonComp<RankComponent>().rankList;

        for (int i = 0; i < m_rankList.Count; i++)
        {
            DestroyItem(m_rankList[i]);
        }

        m_rankList.Clear();

        for (int i = 0; i < rankList.Count; i++)
        {
            UIBase item = CreateItem(c_itemName, "Layout");
            item.SetText("Text_content", rankList[i].nickName + "  " + rankList[i].score);
            if(myID == rankList[i].characterID)
            {
                item.SetActive("Image_BG_me", true);
                item.SetActive("Image_BG_other", false);
            }
            else
            {
                item.SetActive("Image_BG_me", false);
                item.SetActive("Image_BG_other", true);
            }
            m_rankList.Add(item);

            //Debug.Log("m_rankList.Add(item) " + item, item);
        }
    }

    #endregion

    #region 摇杆

    private void InitJoyStick()
    {
        GetJoyStick("MoveStick").onJoyStick = OnJoyStickMove;
        GetJoyStick("MoveStick").ReHomePos();

        GetJoyStick("RotationStick").onJoyStick = OnJoyStickRotation;
        GetJoyStick("RotationStick").ReHomePos();
    }

    void DisposeJoySitck()
    {
        GetJoyStick("MoveStick").onJoyStick = null;
        GetJoyStick("RotationStick").onJoyStick = null;
    }

    /// <summary>
    /// 摇杆移动
    /// </summary>
    /// <param name="dir"></param>
    void OnJoyStickMove(Vector3 dir)
    {
        InputMoveCommand cmd = IInputProxyBase.GetEvent<InputMoveCommand>("InputMoveCommand");
        cmd.m_dir = dir;
        InputManager.Dispatch("InputMoveCommand", cmd);
    }

    /// <summary>
    /// 摇杆旋转
    /// </summary>
    /// <param name="dir"></param>
    void OnJoyStickRotation(Vector3 dir)
    {
        RotationCommand cmd = IInputProxyBase.GetEvent<RotationCommand>("RotationCommand");
        cmd.m_dir = dir;
        InputManager.Dispatch("RotationCommand", cmd);
    }

    #endregion

    #region 元素显示与切换

    List<UIBase> m_elementList = new List<UIBase>();
    public void ElementInit()
    {
        //Debug.Log("ElementInit");

        for (int i = 0; i < 4; i++)
        {
            UIBase uiTmp =  CreateItem(c_elementItem, "layout_element");
            m_elementList.Add(uiTmp);

            uiTmp.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-300 + i * 200, 200, 0);
        }

        InitElementPlanA();

        InitElementPlanB();

        GlobalEvent.AddEvent(GameDataEvent.ElementList, ReceviceElementChange);

        ReceviceElementChange();
    }

    void ElementDispose()
    {
        //Debug.Log("ElementDispose");

        CleanItem();
        m_elementList.Clear();

        GlobalEvent.RemoveEvent(GameDataEvent.ElementList, ReceviceElementChange);
    }

    //NameList保存的是元素的名字
    //List<string> elementIDList = new List<string>();
    List<int> elementList = new List<int>();

    #region 元素切换方案 A

    void InitElementPlanA()
    {
        //AddOnClickListener("Element_1", OnClickElement_1);
        //AddOnClickListener("Element_2", OnClickElement_2);
        //AddOnClickListener("Element_3", OnClickElement_3);
        //AddOnClickListener("Element_4", OnClickElement_4);
    }

    void OnClickElement_1(InputUIOnClickEvent e)
    {
        int index = 1;
        AddElement(index);
        UpdateUI();
    }

    void OnClickElement_2(InputUIOnClickEvent e)
    {
        int index = 2;
        AddElement(index);
        UpdateUI();
    }

    void OnClickElement_3(InputUIOnClickEvent e)
    {
        int index = 3;
        AddElement(index);
        UpdateUI();
    }

    void OnClickElement_4(InputUIOnClickEvent e)
    {
        int index = 4;
        AddElement(index);
        UpdateUI();
    }

    void AddElement(int index)
    {
        if (elementList.Contains(index) 
            && elementList.Count ==2)
        {
            elementList.Remove(index);
        }
        else
        {
            if (elementList.Count > 1)
            {
                elementList.RemoveAt(0);
            }
            elementList.Add(index);
        }
    }

    void UpdateUI()
    {
        for (int i = 1; i <= 2; i++)
        {
            SetActive("Image_BG_" + i, false);
        }

        for (int i = 0; i < elementList.Count; i++)
        {
            SetActive("Image_BG_" + (i + 1), true);
            GetRectTransform("Image_BG_" + (i + 1)).anchoredPosition3D = new Vector3(-500 + elementList[i] * 200, -290, 0);
        }

        //GetRectTransform("")
    }

    #endregion

    #region 元素切换方案 B

    void InitElementPlanB()
    {
        AddDragListener("Image_BG_1", OnDrag);
        AddDragListener("Image_BG_2", OnDrag);

        AddBeginDragListener("Image_BG_1", OnBeginDrag);
        AddBeginDragListener("Image_BG_2", OnBeginDrag);

        AddEndDragListener("Image_BG_1", OnEndDrag);
        AddEndDragListener("Image_BG_2", OnEndDrag);
    }

    Dictionary<string, bool> m_moveDict = new Dictionary<string, bool>();

    void AdsorbLogic(string name,int index)
    {
        if(!m_moveDict.ContainsKey(name))
        {
            m_moveDict.Add(name,false);
        }

        bool isMove = m_moveDict[name];
        RectTransform rect = GetRectTransform(name);

        if (!isMove)
        {
            float aimPosx = rect.anchoredPosition.x;
            float[] step = new float[] { -300, -100, 100, 300 };

            float minDistance = -1;
            int aimIndex = 0;
            for (int i = 0; i < step.Length; i++)
            {
                float dis = Mathf.Abs(aimPosx - step[i]);
                if (minDistance < 0 
                    || dis < minDistance
                    )
                {
                    minDistance = dis;
                    aimIndex = i;
                }
            }

            Vector2 pos = rect.anchoredPosition;
            Vector2 aimPos = pos;
            aimPos.x = step[aimIndex];
            rect.anchoredPosition = Vector2.Lerp(pos, aimPos, Time.deltaTime * 10);

            bool isChange = false;

            if(elementList.Count > index)
            {
                if(elementList[index] != (aimIndex + 1))
                {
                    elementList[index] = (aimIndex + 1);
                    isChange = true;
                }
            }
            else
            {
                elementList.Add(aimIndex + 1);
                isChange = true;
            }

            if(isChange)
            {
                //Debug.Log("AdsorbLogic " + name + " " + elementList[index]);
                ChangeEmement();
            }
        }
    }

    private void OnDrag(InputUIOnDragEvent e)
    {
        Vector2 pos = GetRectTransform(e.m_compName).anchoredPosition;
        pos.x += e.m_delta.x;

        if(pos.x > 300)
        {
            pos.x = 300;
        }

        if(pos.x < -300)
        {
            pos.x = -300;
        }

        GetRectTransform(e.m_compName).anchoredPosition = pos;
    }

    private void OnBeginDrag(InputUIOnBeginDragEvent e)
    {
        if(m_moveDict.ContainsKey(e.m_compName))
        {
            m_moveDict[e.m_compName] = true;
        }
        else
        {
            m_moveDict.Add(e.m_compName, true);
        }
    }

    private void OnEndDrag(InputUIOnEndDragEvent e)
    {
        if (m_moveDict.ContainsKey(e.m_compName))
        {
            m_moveDict[e.m_compName] = false;
        }
        else
        {
            m_moveDict.Add(e.m_compName, false);
        }
    }

    #endregion

    #region 元素切换接口

    List<int> m_choiceList = new List<int>();
    void ChangeEmement()
    {
        m_choiceList.Clear();
        //Debug.Log("ChangeEmement ");

        PlayerComponent pc = m_entity.GetComp<PlayerComponent>();

        for (int i = 0; i < elementList.Count; i++)
        { 
            try
            {
                int elementID = pc.elementData [elementList[i] - 1].id;

                if (!m_choiceList.Contains(elementID)
                    //&& m_playerComp.elementData[elementList[i] - 1].num > 0
                    )
                {
                    m_choiceList.Add(elementID);
                }
            }
            catch
            {
                Debug.LogError((elementList[i] - 1) + " ->" + pc.elementData.Count);
            }
        }

        ApplicationStatusManager.GetStatus<GameStatus>().ChangeElement(m_choiceList);
    }

    #endregion

    #region 元素显示

    void ReceviceElementChange(params object[] objs)
    {
        PlayerComponent pc = m_entity.GetComp<PlayerComponent>();

        if (pc.elementData.Count < 4)
        {
            Debug.LogError("更新元素列表失败！");
            return;
        }

        int haveCount = 0;
        int first = -1;
        int second = -1;

        for (int i = 0; i < pc.elementData.Count; i++)
        {
            UpdateElementUI( m_elementList[i], pc.elementData[i]);

            if(pc.elementData[i].num > 0)
            {
                haveCount++;

                if(haveCount == 1)
                {
                    first = i;
                }
                else if(haveCount == 2)
                {
                    second = i;
                }
            }
        }

        SetDrag(haveCount, first, second);
    }

    void UpdateElementUI(UIBase ui, ElementData info)
    {
        ItemsDataGenerate data = DataGenerateManager<ItemsDataGenerate>.GetData(info.id.ToString());
        ui.SetText("Text_Level", data.m_name + "\n" + info.id);

        //if(info.num == 0)
        //{
        //    ui.GetImage(ui.name).color = Color.red;
        //}
        //else
        //{
        //    ui.GetImage(ui.name).color = Color.white;
        //}
    }

    void SetDrag(int count,int f,int s)
    {
        float[] step = new float[] { -300, -100, 100, 300 };

        if (count > 0)
        {
            if(GetGameObject("Image_BG_1").activeSelf == false)
            {
                SetActive("Image_BG_1", true);
                //锁定第一个
                elementList[0] = f;

                Vector3 pos = GetRectTransform("Image_BG_1").anchoredPosition;
                pos.x = step[f];
                GetRectTransform("Image_BG_1").anchoredPosition = pos;
            }
        }
        else
        {
            SetActive("Image_BG_1", false);
        }

        if (count > 1)
        {
            if (GetGameObject("Image_BG_2").activeSelf == false)
            {
                SetActive("Image_BG_2", true);
                //锁定第二个
                elementList[1] = s;

                Vector3 pos = GetRectTransform("Image_BG_2").anchoredPosition;
                pos.x = step[s];
                GetRectTransform("Image_BG_2").anchoredPosition = pos;

                Debug.Log("s: " + s);
            }
        }
        else
        {
            SetActive("Image_BG_2", false);
        }
    }

    #endregion

    #endregion

    #region 道具

    void ItemInit()
    {
        GlobalEvent.AddEvent(GameDataEvent.ItemList,RecevicePropChange);
    }

    void ItemDispose()
    {
        GlobalEvent.RemoveEvent(GameDataEvent.ItemList, RecevicePropChange);
    }

    List<UIBase> m_PropList = new List<UIBase>();

    void RecevicePropChange(params object[] obj)
    {
        for (int i = 0; i < m_PropList.Count; i++)
        {
            DestroyItem(m_PropList[i]);
        }
        m_PropList.Clear();

        for (int i = 0; i < GameData.ItemList.Count; i++)
        {
            UIBase ui = CreateItem(c_propItem, "ItemList");
            UpdateItemUI(ui, GameData.ItemList[i]);
            ui.AddOnClickListener(c_propItem, ReceviceClickItem, GameData.ItemList[i].id.ToString());

            m_PropList.Add(ui);
        }
    }

    void UpdateItemUI(UIBase ui,p_item_info info)
    {
        ItemsDataGenerate data = DataGenerateManager<ItemsDataGenerate>.GetData(info.id.ToString());

        ui.SetText("Text_number", data.m_name + " " + info.num);
    }

    void ReceviceClickItem(InputUIOnClickEvent e)
    {
        fight_use_item_s msg = new fight_use_item_s();
        msg.id = int.Parse(e.m_pram);

        ProtocolAnalysisService.SendCommand(msg);
    }

    #endregion

    
}