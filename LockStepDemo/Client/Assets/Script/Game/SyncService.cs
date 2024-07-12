using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SyncService 
{
    static RouteRule s_serviceType = RouteRule.Client;
    const float c_syncAheadTime = 0.1f;

    public enum SyncScene
    {
        City,
        Fight,
    }

    private static SyncScene s_currentScene;

    public static SyncScene CurrentScene
    {
        get { return SyncService.s_currentScene; }
        set { SyncService.s_currentScene = value; }
    }

    /// <summary>
    /// 误差允许范围
    /// </summary>
    public const float c_DeviationRange = 3f;

    private static float c_syncOperaTimeSpace = 0.2f;
    private static float c_citySyncOperaTimeSpace = 0.2f;

    const float c_TimeSyncSpace = 300;
    const float c_ReportSpace = 0.1f;

    /// <summary>
    /// 提前多久开始预计算
    /// </summary>
    public static float SyncAheadTime
    {
        get { return SyncService.c_syncAheadTime; }
    }

    static float s_TineSyncTimer = c_TimeSyncSpace;
    static float s_ReportTimer = c_ReportSpace;

    /// <summary>
    /// 两次同步操作的间隔时间
    /// </summary>
    public static float SyncOperaTimeSpace
    {
        get 
        { 
            if(s_currentScene == SyncScene.City)
            {
                return SyncService.c_citySyncOperaTimeSpace; 
            }
            else
            {
                return SyncService.c_syncOperaTimeSpace; 
            }
        }
    }

    /// <summary>
    /// 当前同步类型
    /// </summary>
    public static RouteRule ServiceType
    {
        get { return SyncService.s_serviceType; }
        set 
        { 
            SyncService.s_serviceType = value;
            GlobalEvent.DispatchEvent(SyncEventEnum.ServiceChange);
        }
    }

    private static float m_CurrentServiceTime;

    public static float CurrentServiceTime
    {
        get { return m_CurrentServiceTime; }
    }
    public static float m_ping;

    public static void Init()
    {
        InputManager.AddListener<InputNetworkMessageEvent>("room2_changeserver", ReceviceChangeService);

        InputManager.AddListener<InputNetworkMessageEvent>("time_syncreturn", ReceviceTimeSyncMassage_Client);
        InputManager.AddListener<InputNetworkMessageEvent>("time_sync", ReceviceTimeSyncMassage_Service);
        InputManager.AddListener<InputNetworkMessageEvent>("time_resync", ReceviceTimeReSyncMassage);
        InputManager.AddListener<InputNetworkMessageEvent>("room2_timesync", ReceviceTimeSyncMessageByCity);

        m_CurrentServiceTime = 0;

        ApplicationManager.s_OnApplicationUpdate += OnUpdate;

        if (ApplicationManager.AppMode != AppMode.Release)
        {
            //ApplicationManager.s_OnApplicationOnGUI += OnGUI;
            DevelopReplayManager.s_ProfileGUICallBack += OnGUI;
        }
    }

    public static void Dispose()
    {
        InputManager.RemoveListener<InputNetworkMessageEvent>("room2_changeserver", ReceviceChangeService);

        InputManager.RemoveListener<InputNetworkMessageEvent>("time_syncreturn", ReceviceTimeSyncMassage_Client);
        InputManager.RemoveListener<InputNetworkMessageEvent>("time_sync", ReceviceTimeSyncMassage_Service);
        InputManager.RemoveListener<InputNetworkMessageEvent>("time_resync", ReceviceTimeReSyncMassage);
        InputManager.RemoveListener<InputNetworkMessageEvent>("room2_timesync", ReceviceTimeSyncMessageByCity);

        m_CurrentServiceTime = 0;

        ApplicationManager.s_OnApplicationUpdate -= OnUpdate;

        if (ApplicationManager.AppMode != AppMode.Release)
        {
            //ApplicationManager.s_OnApplicationOnGUI -= OnGUI;
            DevelopReplayManager.s_ProfileGUICallBack -= OnGUI;
        }
    }

    /// <summary>
    /// 发送对时消息
    /// </summary>
    public static void SendTimeSyncMessage()
    {
        if (ServiceType == RouteRule.Client)
        {
            Dictionary<string, object> data = HeapObjectPool.GetSODict();
            data.Add("sendtime", Time.realtimeSinceStartup);

            NetworkManager.SendMessage("time_sync",data);

        }
    }

    public static void SendTimeSyncMessageByCity()
    {
        Dictionary<string, object> data = HeapObjectPool.GetSODict();
        data.Add("client_time", Time.realtimeSinceStartup);

        NetworkManager.SendMessage("room2_timesync", data);
    }

    /// <summary>
    /// 发送重对时消息
    /// </summary>
    static void SendReTimeReSyncMessage()
    {
        //if (ServiceType == RouteRule.Service)
        //{
        //    Dictionary<string, object> data = HeapObjectPool.GetSODict();

        //    NetworkManager.SendMessage("time_resync", data);
        //}
    }

    static void ReceviceTimeSyncMessageByCity(InputNetworkMessageEvent e)
    {
        float sendTime = float.Parse(e.Data["client_time"].ToString());
        float serviceTime = float.Parse(e.Data["server_time"].ToString());
        float currentTime = Time.realtimeSinceStartup;

        m_ping = (currentTime - sendTime) / 2;

        m_CurrentServiceTime = serviceTime + m_ping;
    }

    /// <summary>
    /// 接收对时消息
    /// </summary>
    /// <param name="e"></param>
    static void ReceviceTimeSyncMassage_Client(InputNetworkMessageEvent e)
    {
        //Debug.Log("ReceviceTimeSyncMassage_Client");

        if (ServiceType == RouteRule.Client)
        {
            float sendTime = float.Parse(e.Data["sendtime"].ToString());
            float serviceTime = float.Parse(e.Data["servicetime"].ToString());
            float currentTime = Time.realtimeSinceStartup;

            m_ping = (currentTime - sendTime) / 2;

            m_CurrentServiceTime = serviceTime + m_ping;

            //Debug.Log("ReceviceTimeSyncMassage_Client sendTime: " + sendTime
            //    + " serviceTime: " + serviceTime
            //    + " currentTime " + currentTime);
        }
    }

    /// <summary>
    /// 接收重对时消息
    /// </summary>
    /// <param name="e"></param>
    static void ReceviceTimeReSyncMassage(InputNetworkMessageEvent e)
    {
        if (ServiceType == RouteRule.Client)
        {
            SendTimeSyncMessage();
        }
    }

    /// <summary>
    /// 主机处理对时消息
    /// </summary>
    /// <param name="e"></param>
    static void ReceviceTimeSyncMassage_Service(InputNetworkMessageEvent e)
    {
        //Debug.Log("ReceviceTimeSyncMassage_Service");

        if (ServiceType == RouteRule.Service)
        {
            Dictionary<string, object> data = HeapObjectPool.GetSODict();

            data.Add("sendTime", float.Parse(e.Data["sendTime"].ToString()));
            data.Add("serviceTime", m_CurrentServiceTime);

            NetworkManager.SendMessage("time_syncreturn", data);

            Debug.Log("ReceviceTimeSyncMassage_Service : sendTime :" + float.Parse(e.Data["sendTime"].ToString())
                + "serviceTime " + Time.realtimeSinceStartup);
        }
    }


    public static void ReceviceClientTime(float clientTime)
    {
        if (clientTime > m_CurrentServiceTime)
        {
            //强制重新对时
            SendReTimeReSyncMessage();
        }
    }

    /// <summary>
    /// 更新延迟数据
    /// </summary>
    /// <param name="time"></param>
    public static void UpdatePing(float time)
    {
        m_ping = m_CurrentServiceTime - time;
    }

    static void OnUpdate()
    {
       
        m_CurrentServiceTime += Time.deltaTime;

        //Debug.Log("Sync OnUpdate " + m_CurrentServiceTime);

        s_TineSyncTimer -= Time.deltaTime;

        if(s_TineSyncTimer <0)
        {
            s_TineSyncTimer = c_TimeSyncSpace;
            SendTimeSyncMessage();
        }

        if (s_ReportTimer < 0)
        {
            SendClientStatus();
            s_ReportTimer = c_ReportSpace;
            //SendTimeSyncMessage();
        }
    }

    static void ReceviceChangeService(InputNetworkMessageEvent e)
    {
        bool isService = (bool)e.Data["server"];

        if (isService)
        {
            ServiceType = RouteRule.Service;
        }
        else
        {
            SendTimeSyncMessage();
            ServiceType = RouteRule.Client;
        }
    }

    static void SendClientStatus()
    {
        Dictionary<string, object> data = HeapObjectPool.GetSODict();
        data.Add("time", CurrentServiceTime);

        NetworkManager.SendMessage("room2_report", data);
    }

    static void OnGUI()
    {
        GUILayout.TextField("Ping: " + (m_ping * 1000).ToString("F") + "ms CurrentServiceTime: " + m_CurrentServiceTime.ToString("F") +" ServiceType: " + ServiceType);
    }
}

public enum SyncEventEnum
{
    ServiceChange,
}
