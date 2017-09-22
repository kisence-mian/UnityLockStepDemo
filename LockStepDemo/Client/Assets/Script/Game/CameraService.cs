using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraService : MonoBehaviour
{
    #region 常量

    //public Vector3 c_v3_cameraOffset = new Vector3(-5.29f, 6.62f, -1.67f);//摄像机偏移
    //public  Vector3 c_v3_cameraEugle = new Vector3(45, 75, 0);//摄像机偏移

    //public Vector3 c_v3_cameraOffset = new Vector3(-10.18f, 7.8f, -2.49f);//摄像机偏移
    //public Vector3 c_v3_cameraEugle = new Vector3(45, 75, 0);//摄像机偏移

    //public Vector3 c_v3_cameraOffset = new Vector3(-5.5f, 6.5f, -4.68f);//摄像机偏移
    //public Vector3 c_v3_cameraEugle = new Vector3(33.7f, 48.42f, 0);//摄像机偏移

    static Vector3 c_v3_cameraOffset = new Vector3(5.1f, 8.12f, -5.82f);//摄像机偏移
    static Vector3 c_v3_cameraEugle = new Vector3(50.3f, 317.4f, 0);//摄像机偏移

    //private float c_pov = 49.7f;
    public Vector3 nowCameraOffset;
    public Vector3 nowCameraEugle;


    const string c_s_winAnimName = "win";
    const string c_s_loseAnimName = "lose";
    const string c_s_normalAnimName = "normal";

    const float c_n_cclusionInterval = 0.2f;//遮挡剔除检查间隔时间

    const float c_softFollowSpeed = 12;
    #endregion

    public int m_targetID = -1;
    public GameObject m_target;

    private Transform m_cameraRoot;
    private Transform m_animRoot;
    public GameObject m_mainCameraGo;
    public Camera m_mainCamera;

    private Animator m_CameraAnimator;
    private CameraShoke m_cameraShoke;
    private Vector3 m_v3_nowPos;
    private Vector3 m_v3_nowEugle;

    private CameraStatus m_cameraStatus;
    private Transform m_lookAtAim;

    #region 单例

    private static CameraService instance;

    public static CameraService Instance
    {
        get
        {
            if (instance == null)
            {
                //Debug.Log("instance == null");

                instance = FindObjectOfType<CameraService>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "CameraService";
                    instance = obj.AddComponent<CameraService>();

                    //DontDestroyOnLoad(obj);
                }

                //Debug.Log("instance == " + instance, instance);

                //DontDestroyOnLoad(instance);
                instance.Init();
            }
            return instance;
        }
    }

    #endregion

    #region 初始化

    public void Init()
    {
        if (m_cameraRoot == null)
        {
            m_cameraRoot = GameObject.Find("CameraRoot").transform;
            m_animRoot = m_cameraRoot.Find("AnimRoot");

            m_CameraAnimator = m_animRoot.GetComponent<Animator>();
            m_mainCameraGo = Instance.gameObject;
            m_mainCamera = m_mainCameraGo.GetComponent<Camera>();

            m_cameraShoke = m_mainCameraGo.GetComponent<CameraShoke>();
            if (m_cameraShoke == null)
            {
                m_cameraShoke = m_mainCameraGo.AddComponent<CameraShoke>();
            }
            m_cameraShoke.Init(m_mainCameraGo.gameObject);

        }

        InitCameraOffsetAndEugle();
        //m_targetID = GameLogic.MyPlayerID;
        m_target = null;
        EnterFollowStatus();

        AnimRehome();

        //GlobalEvent.AddEvent(UserData.UserDataChangeEvent.PlayerID, RecevicePlayerIDChange);
    }
    private void InitCameraOffsetAndEugle()
    {
        SetCameraOffsetAndEugle(c_v3_cameraOffset, c_v3_cameraEugle);
    }

    public void Dispose()
    {
        m_cameraStatus = CameraStatus.None;
        m_showList.Clear();
        m_hideList.Clear();
        Destroy(circleRotateHelper);
    }

    /// <summary>
    /// 抖动主摄像机
    /// </summary>
    /// <param name="抖动的时间长度"></param>
    public void ShokeMainCamera(float l_n_shokeTime, float l_n_amount, float l_n_decreaseFactor, Vector3 l_v3_randomOffest, Vector3 l_v3_weight)
    {
        m_cameraShoke.Shoke(l_n_shokeTime, l_n_amount, l_n_decreaseFactor, l_v3_randomOffest, l_v3_weight);
    }

    void Update()
    {
        if (m_target != null)
        {
        }
        else
        {
            FindTargetCharacter(m_targetID);
        }

        switch (m_cameraStatus)
        {
            case CameraStatus.Follow:
                OnFollowStatus();
                break;
            case CameraStatus.RootAnim:
                OnRootAnim();
                break;

        }

        OcclusionCulling();

    }

    private void FindTargetCharacter(int l_targetID)
    {
        //if (CharacterManager.GetCharacterIsExit(l_targetID))
        //{
        //    m_target = CharacterManager.GetCharacter(l_targetID).gameObject;
        //    Vector3 l_cameraOffset = m_target.transform.position + nowCameraOffset;
        //    Vector3 l_cameraEugle = nowCameraEugle;
        //    m_cameraRoot.position = l_cameraOffset;
        //    m_cameraRoot.eulerAngles = l_cameraEugle;
        //}

    }

    #endregion

    #region 摄像机状态部分


    private void EnterFollowStatus()
    {
        m_cameraStatus = CameraStatus.Follow;
    }

    private void EnterRootAnimStatus()
    {
        m_cameraStatus = CameraStatus.RootAnim;
    }


    private void OnFollowStatus()
    {
        if (m_target != null)
        {
            m_v3_nowEugle = m_cameraRoot.eulerAngles;
            m_v3_nowPos = m_cameraRoot.position;
            Vector3 l_cameraOffset = Vector3.Lerp(m_v3_nowPos, m_target.transform.position + nowCameraOffset, Time.deltaTime * c_softFollowSpeed);

            Vector3 l_cameraEugle = Vector3.Lerp(m_v3_nowEugle, nowCameraEugle, Time.deltaTime * c_softFollowSpeed);

            //l_cameraOffset = m_target.transform.position + m_CameraOffset;//紧密跟随
            m_cameraRoot.position = l_cameraOffset;
            m_cameraRoot.eulerAngles = l_cameraEugle;


            if (m_cameraShoke != null)
            {
                m_cameraShoke.UpdateShoke();
            }
        }
    }

    private void OnRootAnim()
    {
        if (m_lookAtAim != null)
        {
            m_cameraRoot.LookAt(m_lookAtAim);
        }

    }

    private void OnStoryAnim()
    {

    }

    #endregion

    #region 摄像机技能动画部分

    public void PlaySkillAnim(string l_s_pathName)
    {
        if (m_cameraStatus != CameraStatus.RootAnim)
        {
        }

    }

    private void PlayAnimator(string l_s_pathName)
    {
        if (l_s_pathName != "null")
        {
            m_CameraAnimator.CrossFade(l_s_pathName, 0.1f, 0, 0.1f);
            Invoke("EnterFollowStatus", 0.3f);

        }
        else
        {
            EnterFollowStatus();

        }

    }

    //归位
    public void AnimRehome()
    {
        PlayAnimator(c_s_normalAnimName);
    }

    /// <summary>
    /// 播放胜利镜头动画
    /// </summary>
    public void PlayWinAnim()
    {
        PlayAnimator(c_s_winAnimName);
    }


    /// <summary>
    /// 播放胜利镜头动画
    /// </summary>
    public void PlayLoseAnim()
    {
        PlayAnimator(c_s_loseAnimName);
    }


    #region 原先的镜头动画实现

    //OnEndCallback mOnEndCallback;

    //private Vector3 m_cameraPos;
    //private Vector3 m_cameraEugle;

    //private void PlayAnim(string l_s_pathName, float l_s_timeScale, OnEndCallback callBack)
    //{
    //    m_cameraPos = transform.position;
    //    m_cameraEugle = transform.eulerAngles;
    //    GameObject l_go_path = GameObjectManager.CreateGameObjectByPool(l_s_pathName);
    //    l_go_path.transform.position = m_cameraPos;
    //    l_go_path.transform.eulerAngles = m_cameraEugle;
    //    l_go_path.transform.localScale = Vector3.one;

    //    if (l_go_path.GetComponent<MoveData>().Durations.Length > 0)
    //    {
    //        SplineController l_splineController = transform.GetComponent<SplineController>();
    //        if (l_splineController == null)
    //        {
    //            l_splineController = gameObject.AddComponent<SplineController>();
    //        }
    //        l_splineController.Durations = l_go_path.GetComponent<MoveData>().Durations;
    //        l_splineController.SplineRoot = l_go_path;

    //        l_splineController.MyStart(callBack);
    //        Time.timeScale = l_s_timeScale;
    //    }

    //    else
    //    {
    //        GameObjectManager.DestroyGameobjectByPool(l_go_path);
    //        EnterFollowStatus();
    //        //Debug.LogError(l_s_skillID + "无镜头动画：" + l_s_pathName);
    //        return;

    //    }
    //}

    //void PlayAnimEndCallBack()
    //{
    //    EnterFollowStatus();
    //}






    #endregion

    #endregion

    #region 自由调用部分 

    /// <summary>
    /// 一段移动
    /// </summary>
    /// <param name="toPos"></param>目标位置
    /// <param name="time"></param>时间
    /// <param name="fromPos"></param>起始位置，用于预览
    /// <param name="interp"></param>差值方式
    /// <param name="callBack"></param>回调，缺省
    public void MoveTo(Vector3 toPos, float time, Vector3? fromPos = null, InterpType interp = InterpType.Default, AnimCallBack callBack = null, object[] parameter = null)
    {
        EnterRootAnimStatus();

        if (fromPos == null)
        {
            AnimSystem.Move(animObject: m_cameraRoot.gameObject, from: m_cameraRoot.position, to: toPos, time: time, interp: interp, callBack: callBack, parameter: parameter);
        }
        else
        {
            AnimSystem.Move(animObject: m_cameraRoot.gameObject, from: fromPos, to: toPos, time: time, interp: interp, callBack: callBack, parameter: parameter);
        }


    }

    /// <summary>
    /// 多阶移动
    /// </summary>
    public void MoveToMore(CameraAniInfo[] infoList, Vector3? fromPos = null)
    {
        int index = 0;
        int maxIndex = infoList.Length;
        object[] parms = new object[3];
        parms[0] = index;
        parms[1] = maxIndex;
        parms[2] = infoList;
        MoveTo(infoList[index].toV3, infoList[index].time, fromPos, infoList[index].interpType, MoveToCallBack, parms);
    }

    /// <summary>
    /// 多阶移动的回调
    /// </summary>
    /// <param name="parameter"></param>
    private void MoveToCallBack(object[] parameter)
    {
        int index = (int)parameter[0] + 1;
        int maxIndex = (int)parameter[1];
        CameraAniInfo[] infoList = (CameraAniInfo[])parameter[2];

        parameter[0] = index;
        if (index < maxIndex)
        {
            MoveTo(infoList[index].toV3, infoList[index].time, null, infoList[index].interpType, MoveToCallBack, parameter);
        }


    }


    /// <summary>
    /// 一段旋转
    /// </summary>

    public void RotateTo(Vector3 toEugle, float time, Vector3? fromPos = null, InterpType interp = InterpType.Default, AnimCallBack callBack = null, object[] parameter = null)
    {

        EnterRootAnimStatus();

        Vector3 newToEugle = LimitEugle(toEugle);
        Vector3? newFromEugle = fromPos;

        if (fromPos == null)
        {
            newFromEugle = LimitEugle(m_cameraRoot.eulerAngles);
        }
        AnimSystem.Rotate(animObject: m_cameraRoot.gameObject, from: newFromEugle, to: newToEugle, time: time, interp: interp, callBack: callBack, parameter: parameter);

    }

    /// <summary>
    /// 多阶旋转
    /// </summary>

    public void RotateToMore(CameraAniInfo[] infoList, Vector3? fromPos = null)
    {
        int index = 0;
        int maxIndex = infoList.Length;
        object[] parms = new object[3];
        parms[0] = index;
        parms[1] = maxIndex;
        parms[2] = infoList;
        RotateTo(infoList[index].toV3, infoList[index].time, fromPos, infoList[index].interpType, RotateToCallBack, parms);
    }


    /// <summary>
    /// 多阶旋转的回调
    /// </summary>
    /// <param name="parameter"></param>
    private void RotateToCallBack(object[] parameter)
    {
        int index = (int)parameter[0] + 1;
        int maxIndex = (int)parameter[1];
        CameraAniInfo[] infoList = (CameraAniInfo[])parameter[2];

        parameter[0] = index;
        if (index < maxIndex)
        {
            RotateTo(infoList[index].toV3, infoList[index].time, null, infoList[index].interpType, RotateToCallBack, parameter);
        }

    }


    GameObject circleRotateHelper;

    public GameObject CircleRotateHelfer
    {
        get
        {
            if (circleRotateHelper == null)
            {
                circleRotateHelper = new GameObject();
                circleRotateHelper.name = "circleRotateHelper";
            }

            return circleRotateHelper;
        }

    }

    /// <summary>
    /// 多阶圆形旋转
    /// </summary>

    public void CircleRotate(CameraAniInfo[] infoList, Vector3 center, float radius, Vector3? fromPos = null)
    {
        CircleRotateHelfer.transform.position = center;

        CircleRotateHelfer.transform.LookAt(m_cameraRoot.transform);

        CircleRotateHelfer.transform.eulerAngles = new Vector3(0, CircleRotateHelfer.transform.eulerAngles.y, 0);

        m_cameraRoot.parent = CircleRotateHelfer.transform;

        int index = -1;
        int maxIndex = infoList.Length;
        object[] parms = new object[3];
        parms[0] = index;
        parms[1] = maxIndex;
        parms[2] = infoList;

        AnimSystem.Move(animObject: m_cameraRoot.gameObject, from: m_cameraRoot.transform.localPosition, to: new Vector3(0, 0, radius), time: 1, isLocal: true, callBack: CircleRotateCallBack, parameter: parms);
    }



    /// <summary>
    /// 多阶旋转的回调
    /// </summary>
    /// <param name="parameter"></param>
    private void CircleRotateCallBack(object[] parameter)
    {
        int index = (int)parameter[0] + 1;
        int maxIndex = (int)parameter[1];
        CameraAniInfo[] infoList = (CameraAniInfo[])parameter[2];

        parameter[0] = index;
        if (index < (maxIndex - 1))
        {
            AnimSystem.Rotate(animObject: circleRotateHelper.gameObject, from: null, to: infoList[index].toV3, time: infoList[index].time, interp: infoList[index].interpType, callBack: CircleRotateCallBack, parameter: parameter);
        }
        else if (index < maxIndex)
        {
            AnimSystem.Rotate(animObject: circleRotateHelper.gameObject, from: null, to: infoList[index].toV3, time: infoList[index].time, interp: infoList[index].interpType, callBack: CircleRotateCallBackEnd, parameter: parameter);
        }

    }

    //最后一次旋转完毕后，取出camera
    private void CircleRotateCallBackEnd(object[] parameter)
    {
        m_cameraRoot.SetParent(null);
    }

    private Vector3 LimitEugle(Vector3 oldEugle)
    {
        Vector3 newEugle = oldEugle;

        if (newEugle.y > 180)
        {
            newEugle.y -= 360;
            newEugle = LimitEugle(newEugle);
        }

        if (newEugle.y < -180)
        {
            newEugle.y += 360;
            newEugle = LimitEugle(newEugle);
        }

        return newEugle;
    }

    //镜头的位置和旋转回到跟随位置
    public void ReHomePosAndEugle()
    {
        EnterFollowStatus();
    }

    public void LookAt(Transform aim)
    {
        EnterRootAnimStatus();
        m_lookAtAim = aim;
    }

    public void StopLookAt()
    {
        EnterFollowStatus();
        m_lookAtAim = null;
    }


    public void FollowAnyOne(int targetID)
    {
        FindTargetCharacter(targetID);
    }

    public void ResetFollow()
    {
        FindTargetCharacter(m_targetID);
    }


    public void SetCameraOffsetAndEugle(Vector3 offset, Vector3 eugle)
    {
        nowCameraOffset = offset;
        nowCameraEugle = eugle;
    }


    ////测试
    //void OnGUI()
    //{
    //    //if (GUI.Button(new Rect(120, 10, 200, 100), "移动"))
    //    //{
    //    //    MoveTo(new Vector3(14.62f, 3.8f, -2.29f), 2);
    //    //}

    //    //if (GUI.Button(new Rect(120, 120, 200, 100), "旋转"))
    //    //{
    //    //    RotateTo(new Vector3(2.09f, 75, 0), 2);
    //    //}

    //    //if (GUI.Button(new Rect(120, 200, 200, 100), "归位"))
    //    //{
    //    //    ReHomePosAndEugle();
    //    //}
    //    //if (GUI.Button(new Rect(120, 10, 200, 100), "跟随"))
    //    //{
    //    //    FollowAnyOne(1);
    //    //}

    //    //if (GUI.Button(new Rect(120, 120, 200, 100), "复位"))
    //    //{
    //    //    ResetFollow();
    //    //}
    //    //if (GUI.Button(new Rect(120, 120, 200, 100), "showAnim"))
    //    //{
    //    //    DisplayManager.PlayShowAni(3);
    //    //}

    //}

    #endregion

    #region 遮挡剔除部分


    private float m_n_cclusionTimer;

    private List<GameObject> m_hideList = new List<GameObject>();
    private List<GameObject> m_showList = new List<GameObject>();

    Vector3 playerLeftPos = new Vector3(-1.34f, 0, 2.5f);
    Vector3 playerRightPos = new Vector3(2.32f, 0, -1.8f);


    string[] changeTex = { "_MainTex", "_LightMap" };

    public int m_hideLayer = 18;
    private void OcclusionCulling()
    {

    }


    private void FindHideObj(Vector3 fromPos, Vector3 toPos, float rayLength)
    {
        RaycastHit[] m_rayHits = Physics.RaycastAll(fromPos, toPos, rayLength);
        //Vector3 end = l_v3_cameraPosition + (m_v3_PlayerPos - l_v3_cameraPosition).normalized * m_n_rayLength;

        //Debug.DrawLine(l_v3_cameraPosition, end, Color.blue);

        for (int i = 0; i < m_rayHits.Length; i++)
        {
            RaycastHit hit = m_rayHits[i];
            if (hit.collider.gameObject.layer == m_hideLayer)
            {
                if (hit.collider.gameObject.transform.childCount > 0)
                {
                    GameObject hideObj = hit.collider.gameObject.transform.GetChild(0).gameObject;
                    if (!m_hideList.Contains(hideObj))
                    {
                        m_hideList.Add(hideObj);
                    }
                }

            }
        }
    }

    #endregion

    #region 事件接收

    void RecevicePlayerIDChange(params object[] objs)
    {
        //m_targetID = UserData.PlayerID;
        //m_target = null;
    }

    #endregion

}
[System.Serializable]
public class CameraAniInfo
{

    public Vector3 toV3;
    public float time;
    public InterpType interpType;

    public CameraAniInfo() { }

    public CameraAniInfo(Vector3 l_toV3, float l_time, InterpType l_interpType = InterpType.Default)
    {
        toV3 = l_toV3;
        time = l_time;
        interpType = l_interpType;
    }
}

public enum CameraStatus
{
    None,
    Follow,
    RootAnim,
}
