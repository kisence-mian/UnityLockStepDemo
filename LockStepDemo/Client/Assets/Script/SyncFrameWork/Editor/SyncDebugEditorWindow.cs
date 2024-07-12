using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SyncDebugEditorWindow : EditorWindow
{
    [MenuItem("Window/同步监视器", priority = 301)]
    public static void ShowWindow()
    {
        GetWindow(typeof(SyncDebugEditorWindow));
    }

    #region 生命周期

    void OnEnable()
    {
        EditorGUIStyleData.Init();

        GlobalEvent.AddTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.AddTypeEvent<PursueMsg>(RecevicePursueMsg);
        GlobalEvent.AddTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
        GlobalEvent.AddTypeEvent<StartSyncMsg>(ReceviceStartSyncMsg);
        GlobalEvent.AddTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
        GlobalEvent.AddTypeEvent<CommandComponent>(ReceviceCommandMsg);

        GlobalEvent.AddEvent(SyncDebugSystem.c_isAllMessage, ReceviceAllMsg);
        GlobalEvent.AddEvent(SyncDebugSystem.c_isConflict, ReceviceConflict);
        GlobalEvent.AddEvent(SyncDebugSystem.c_Recalc, ReceviceRecalc);
    }

    private void OnDestroy()
    {
        GlobalEvent.RemoveTypeEvent<SyncEntityMsg>(ReceviceSyncEntity);
        GlobalEvent.RemoveTypeEvent<PursueMsg>(RecevicePursueMsg);
        GlobalEvent.RemoveTypeEvent<ChangeSingletonComponentMsg>(ReceviceChangeSingletonCompMsg);
        GlobalEvent.RemoveTypeEvent<StartSyncMsg>(ReceviceStartSyncMsg);
        GlobalEvent.RemoveTypeEvent<AffirmMsg>(ReceviceAffirmMsg);
        GlobalEvent.RemoveTypeEvent<CommandComponent>(ReceviceCommandMsg);

        GlobalEvent.RemoveEvent(SyncDebugSystem.c_isAllMessage, ReceviceAllMsg);
        GlobalEvent.RemoveEvent(SyncDebugSystem.c_isConflict, ReceviceConflict);
        GlobalEvent.RemoveEvent(SyncDebugSystem.c_Recalc, ReceviceRecalc);
    }

    private void Update()
    {
        Repaint();
    }

    #endregion

    #region Data
    WorldBase m_aimWorld;
    Dictionary<int,SyncDebugData> m_dataDict = new Dictionary<int, SyncDebugData>();

    public void GetWorld(int index)
    {
        if (WorldManager.WorldList.Count > index)
        {
            m_aimWorld = WorldManager.WorldList[index];
        }
        else
        {
            Debug.LogError("获取世界失败！");
        }
    }

    public SyncDebugData GetSyncData(int frame)
    {
        if(m_dataDict.ContainsKey(frame))
        {
            return m_dataDict[frame];
        }
        else
        {
            SyncDebugData data = new SyncDebugData();
            m_dataDict.Add(frame, data);
            return data;
        }
    }

    #endregion

    #region GUI

    private string[] toolbarTexts = { "基础数据", "帧流" ,"历史数据"};
    private int toolbarOption = 0;

    void OnGUI()
    {
        titleContent.text = "同步监视器";
        if (m_aimWorld != null)
        {
            toolbarOption = GUILayout.Toolbar(toolbarOption, toolbarTexts, GUILayout.Width(Screen.width));
            switch (toolbarOption)
            {
                case 0:
                    BaseDataGUI(); break;

                case 1:
                    FrameStreamGUI(); break;

                case 2:
                    HistoryDataGUI(); break;
            }
        }
        else
        {
            GetWorldGUI();
        }
    }

    int worldIndex = 0;
    void GetWorldGUI()
    {
        worldIndex = EditorGUILayout.IntField("世界序号", worldIndex);

        if(GUILayout.Button("获取世界"))
        {
            GetWorld(worldIndex);
            m_dataDict.Clear();
        }
    }

    void BaseDataGUI()
    {
        GUILayout.Label("RTT :" + m_aimWorld.GetSingletonComp<ConnectStatusComponent>().rtt);
        GUILayout.Label("Frame :" + m_aimWorld.FrameCount);
        GUILayout.Label("AheadFrame :" + m_aimWorld.GetSingletonComp<ConnectStatusComponent>().aheadFrame);
        GUILayout.Label("ClearFrame :" + m_aimWorld.GetSingletonComp<ConnectStatusComponent>().ClearFrame);
        GUILayout.Label("isAllFrame :" + m_aimWorld.GetSingletonComp<ConnectStatusComponent>().isAllFrame);
        GUILayout.Label("未确认帧数 :" + m_aimWorld.GetSingletonComp<ConnectStatusComponent>().unConfirmFrame.Count);
    }

    const int ShowCount = 50;
    int ShowRow  = 3;
    Texture2D showTexture;
    void FrameStreamGUI()
    {
        int start = m_aimWorld.FrameCount - ShowCount;
        if(start < 0)
        {
            start = 0;
        }

        showTexture = new Texture2D(ShowCount * 20, 20);

        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

        SyncDebugData data = GetSyncData(start);
        data.isConflict = true;

        for (int i = start; i < m_aimWorld.FrameCount; i++)
        {
            ShowFrameDataGUI(showTexture,i,  i - start);
        }

        showTexture.Apply();
        GUILayout.Box(showTexture);

        GUILayout.Label(m_aimWorld.FrameCount.ToString());

        GUILayout.EndHorizontal();
    }

    void ShowFrameDataGUI(Texture2D tex,int frame,int index)
    {
        SyncDebugData data = GetSyncData(frame);
        ChangeShowTexture(tex, index, data);
    }

    Texture2D ChangeShowTexture(Texture2D tex, int index, SyncDebugData data)
    {
        if(data.AffirmMsgCount > 0)
        {
            SetColor(tex, index, 1, Color.grey);
        }
        else
        {
            SetColor(tex, index, 1, Color.green);
        }

        if (data.isAllMessage)
        {
            SetColor(tex, index, 2, Color.green);
        }
        else
        {
            SetColor(tex, index, 2, Color.grey);
        }

        //3位置给重计算
        if (data.RecalcCount ==  0)
        {
            SetColor(tex, index, 3, Color.grey);
        }
        else if(data.RecalcCount == 0)
        {
            SetColor(tex, index, 3, Color.blue);
        }
        else
        {
            SetColor(tex, index, 3, Color.red);
        }

        //4 位置 给特殊位置
        if (data.PursueCount > 0)
        {
            SetColor(tex, index, 4, Color.yellow);
        }
        else
        {
            SetColor(tex, index, 4, Color.green);
        }

        //冲突提示位
        if (data.isConflict)
        {
            SetColor(tex, index, 4, Color.red);
        }

        //SetColor(tex, 1, Color.grey);
        //SetColor(tex, 2, Color.green);
        //SetColor(tex, 3, Color.red);
        //SetColor(tex, 4, Color.blue);

        tex.Apply();

        return tex;
    }

    void SetColor(Texture2D tex,int index,int pos,Color col)
    {
        int x1 = 0;
        int x2 = 0;
        int y1 = 0;
        int y2 = 0;

        if(pos == 1)
        {
            x1 = 0;
            x2 = 10;
            y1 = 10;
            y2 = 20;
        }

        if (pos == 2)
        {
            x1 = 10;
            x2 = 20;
            y1 = 10;
            y2 = 20;
        }

        if (pos == 3)
        {
            x1 = 0;
            x2 = 10;
            y1 = 0;
            y2 = 10;
        }

        if (pos == 4)
        {
            x1 = 10;
            x2 = 20;
            y1 = 0;
            y2 = 10;
        }

        x1 += index * 25;
        x2 += index * 25;

        for (int i = x1; i < x2; i++)
        {
            for (int j = y1; j < y2; j++)
            {
                tex.SetPixel(i,j,col);
            }
        }
    }

    void HistoryDataGUI()
    {

    }


    #endregion

    #region 事件接收

    void ReceviceStartSyncMsg(StartSyncMsg msg, params object[] objs)
    {
        SyncDebugData data = GetSyncData(msg.frame);
        data.StartSyncMsgCount++;
    }

    void RecevicePursueMsg(PursueMsg msg, params object[] objs)
    {
        SyncDebugData data = GetSyncData(msg.frame);
        data.PursueCount++;
    }

    void ReceviceChangeSingletonCompMsg(ChangeSingletonComponentMsg msg, params object[] objs)
    {
        SyncDebugData data = GetSyncData(msg.frame);
        data.ChangeSingletonCompCount++;
    }

    void ReceviceAffirmMsg(AffirmMsg msg, params object[] objs)
    {
        SyncDebugData data = GetSyncData(msg.frame);
        data.AffirmMsgCount++;
    }
    void ReceviceSyncEntity(SyncEntityMsg msg, params object[] objs)
    {
        SyncDebugData data = GetSyncData(msg.frame);
        data.SyncEntityCount++;
    }

    void ReceviceCommandMsg(CommandComponent cmd, params object[] objs)
    {
        SyncDebugData data = GetSyncData(cmd.frame);
        data.CommandList.Add(cmd);
    }

    void ReceviceAllMsg(params object[] objs)
    {
        SyncDebugData data = GetSyncData((int)objs[0]);
        data.isAllMessage = true;
    }

    void ReceviceConflict(params object[] objs)
    {
        SyncDebugData data = GetSyncData((int)objs[0]);
        data.isConflict = true;
    }

    void ReceviceRecalc(params object[] objs)
    {
        SyncDebugData data = GetSyncData((int)objs[0]);
        data.RecalcCount++;
    }

    #endregion

    public class SyncDebugData
    {
        public bool isAllMessage = false;
        public bool isReCalc = false;
        public bool isConflict = false;

        public int RecalcCount = 0;
        public int PursueCount = 0;
        public int StartSyncMsgCount = 0;
        public int ChangeSingletonCompCount = 0;
        public int AffirmMsgCount = 0;
        public int SyncEntityCount = 0;
        public List<CommandComponent> CommandList = new List<CommandComponent>();
    }
}


