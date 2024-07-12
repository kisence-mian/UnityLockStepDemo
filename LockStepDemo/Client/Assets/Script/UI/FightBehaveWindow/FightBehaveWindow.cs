using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FightBehaveWindow : UIWindowBase
{
    #region 属性
    const float n_sliderAniTime = 1;    // 血条动画时间（临时写死，后期需要改为读配置）
    const float n_standardLifeNum = 100;//标准血量
    const float n_standardLifeBGLength = 243.5f;//标准血量(100)的血条背景长度
    const float c_baseScreenWidth = 1920;//基础屏宽分辨率
    public int n_characterID;
    private float n_oldLifeRatio;       //上次的血量比例
    private long n_oldHp = -1;      //上次的血量

    #region 各种UI节点

    private Canvas bloodRoot;//血条根节点
    private Slider lifeSlider;      //即时更新的血条
    private Slider lifeAniSlider;       //带动画的血条

    private RectTransform lifeSliderArea; //血条总长
    private RectTransform ShieldSliderArea; //护盾条总长
    private RectTransform lifeSliderBG; //血条背景（刻度）
    public Camera mainCamera;
    public GameObject go_character;     //角色控制
    private GameObject pre_DisplayTemplate;     // 展示内容的模板
    private Transform tran_follow;      //UI跟随部分


    private GameObject statusSliderObj;
    private Slider statusSlider;//特殊状态进度条
    private Canvas statusSliderCanvas;//特殊状态进度条

    #endregion

    private float statusTimer = 0; //特殊状态计时器
    private float statusTimerStep; //步长
    private bool bloodIsFull = true; //满血状态

    #region EventKey

    string m_InitKey = "";
    string m_MoveKey = "";
    string m_DamageKey = "";
    string m_RecoverKey = "";
    string m_SKillKey = "";
    string m_AttackKey = "";
    string m_BeBreakKey = "";

    #endregion

    #endregion

    #region UI流程

    /// <summary>
    /// UI的初始化请放在这里
    /// </summary>
    public override void OnInit()
    {
        screenScale = c_baseScreenWidth / Screen.width;

        tran_follow = GetGameObject("Follow").transform;
        bloodRoot = GetCanvas("Blood");
        bloodRoot.enabled = false;
        tran_follow.gameObject.SetActive(true);
        InitSpecialSlider();
    }


    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {
        //cd
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        return base.EnterAnim(l_animComplete, l_callBack, objs);
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        return base.ExitAnim(l_animComplete, l_callBack, objs);
    }

    public override void OnClose()
    {
        base.OnClose();
        CharacterManager.RemoveListener(m_InitKey, InitCharacter);
        CharacterManager.RemoveListener(m_MoveKey, RefreshPoition);
        CharacterManager.RemoveListener(m_DamageKey, BeAttack);
        CharacterManager.RemoveListener(m_RecoverKey, BeRecover);

        CharacterManager.RemoveListener(m_SKillKey, UseSkill);
        CharacterManager.RemoveListener(m_AttackKey, UseSkill);
        CharacterManager.RemoveListener(m_BeBreakKey, BeBreak);
    }

    public override void Show()
    {
        m_canvas.enabled = true;
    }

    public override void Hide()
    {
        m_canvas.enabled = false;
    }

    #endregion

    #region Update
    void Update()
    {
        if (UpdateShowOrNot())
        {
            SpecialStatusChange();
            UIFollow();
        }
    }

    #endregion

    #region 初始化

    public void SetCharacterID(int characterID)
    {
        if (CameraService.Instance.m_mainCameraGo == null)
        {
            return;
        }

        mainCamera = CameraService.Instance.m_mainCamera;

        GetEventKey(characterID);

        CharacterManager.AddListener(m_InitKey, InitCharacter);
        CharacterManager.AddListener(m_MoveKey, RefreshPoition);
        CharacterManager.AddListener(m_DamageKey, BeAttack);
        CharacterManager.AddListener(m_RecoverKey, BeRecover);
        CharacterManager.AddListener(m_SKillKey, UseSkill);
        CharacterManager.AddListener(m_AttackKey, UseSkill);
        CharacterManager.AddListener(m_BeBreakKey, BeBreak);

        CharacterBase character = CharacterManager.GetCharacter(characterID);

        go_character = character.gameObject;
        n_oldHp = character.m_Property.MaxHp;
        
        v3_FollowOffset.y = character.m_Property.m_bloodHight* Screen.height;
        n_characterID = characterID;

        bloodIsFull = true;

        UpdateSliderLength(character.m_Property.Shield, character.m_Property.MaxHp);
        InitLifeSlider(character.m_camp);
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    private void InitCharacter(CharacterEventType eventType, CharacterBase character, object[] args)
    {
    }

    /// <summary>
    /// 角色移动时回调
    /// </summary>
    public void RefreshPoition(CharacterEventType eventType, CharacterBase character, object[] args)
    {
        //Debug.Log("character: move" + character);    
    }

    /// <summary>
    /// 角色被攻击回调
    /// </summary>
    public void BeAttack(CharacterEventType eventType, CharacterBase character, object[] args)
    {
        //Debug.Log("BeAttack -->");
        UpdateLifeSlider((float)character.m_Property.HP / (float)character.m_Property.MaxHp);
        UpdateSliderLength(character.m_Property.Shield, character.m_Property.MaxHp);

        TextDisplaySizeType textSizeType = TextDisplaySizeType.Small;

        if ((bool)args[1] == true)
        {
            textSizeType = TextDisplaySizeType.Large;
        }

        DisplayNum(n_oldHp - character.m_Property.HP,character.m_camp, textSizeType, TextDisplayStyleType.Hurt);
        
        n_oldHp = character.m_Property.HP;
    }

    /// <summary>
    /// 角色回复回调
    /// </summary>
    public void BeRecover(CharacterEventType eventType, CharacterBase character, object[] args)
    {
        UpdateLifeSliderRecover((float)character.m_Property.HP / (float)character.m_Property.MaxHp);
        float recoverNum = character.m_Property.HP - n_oldHp;
        if (recoverNum > 0)
        {
            if ((bool)(args[1]) == false || character.m_camp == Camp.Brave)//怪物的自动回血，不显示
            {
                DisplayNum(character.m_Property.HP - n_oldHp, character.m_camp, TextDisplaySizeType.Middle, TextDisplayStyleType.Recover);
            }
           
        }
        n_oldHp = character.m_Property.HP;
    }

    /// <summary>
    /// 角色使用技能
    /// </summary>
    public void UseSkill(CharacterEventType eventType, CharacterBase character, object[] args)
    {
        SkillData skillData = (SkillData)args[0];

        float singleTime = skillData.BeforeTime > 0 ? skillData.HitTime : 0;

        BeginSpecialStatus(singleTime);
    }

    /// <summary>
    /// 被打断
    /// </summary>
    public void BeBreak(CharacterEventType eventType, CharacterBase character, object[] args)
    {
        SkillStatusStop();
    }

    private void InitLifeSlider(Camp characterCamp)
    {
        if (characterCamp == Camp.Brave)
        {
            lifeSlider = GetGameObject("Slider").GetComponent<Slider>();
            lifeAniSlider = GetGameObject("SliderAni").GetComponent<Slider>();
            GetGameObject("Slider").SetActive(true);
            GetGameObject("SliderAni").SetActive(true);
            GetGameObject("mSlider").SetActive(false);
            GetGameObject("mSliderAni").SetActive(false);
            lifeSliderBG = null;
        }
        else
        {
            lifeSlider = GetGameObject("mSlider").GetComponent<Slider>();
            lifeAniSlider = GetGameObject("mSliderAni").GetComponent<Slider>();
            GetGameObject("Slider").SetActive(false);
            GetGameObject("SliderAni").SetActive(false);
            GetGameObject("mSlider").SetActive(true);
            GetGameObject("mSliderAni").SetActive(true);
            lifeSliderBG = lifeAniSlider.transform.Find("Background").GetComponent<RectTransform>();
            lifeSliderArea = lifeSlider.transform.Find("Fill Area").GetComponent<RectTransform>();
            ShieldSliderArea = lifeSlider.transform.Find("Fill Area/FillMask/Shield").GetComponent<RectTransform>();
        }

        n_oldLifeRatio = 1;
        SetLifeSliderValue(1);
        SetLifeSliderAniValue(1);
    }

    private void InitSpecialSlider()
    {
        statusSliderObj = GetGameObject("StatusSlider");
        statusSlider = GetSlider("StatusSlider");
        statusSliderCanvas = GetCanvas("StatusSlider");
        BeginSpecialStatus(0);
    }

    void GetEventKey(int characterID)
    {
        m_InitKey = GameUtils.GetEventKey(characterID, CharacterEventType.Init);
        m_MoveKey = GameUtils.GetEventKey(characterID, CharacterEventType.Move);
        m_DamageKey = GameUtils.GetEventKey(characterID, CharacterEventType.Damage);
        m_RecoverKey = GameUtils.GetEventKey(characterID, CharacterEventType.Recover);
        m_SKillKey = GameUtils.GetEventKey(characterID, CharacterEventType.SKill);
        m_AttackKey = GameUtils.GetEventKey(characterID, CharacterEventType.Attack);
        m_BeBreakKey = GameUtils.GetEventKey(characterID, CharacterEventType.BeBreak);
    }

    #endregion

    #region ECS

    int m_entityID;

    public void SetEntity(EntityBase entity)
    {
        if (CameraService.Instance.m_mainCameraGo == null)
        {
            return;
        }

        PerfabComponent pc = entity.GetComp<PerfabComponent>();
        LifeComponent lc = entity.GetComp<LifeComponent>();
        PlayerComponent plc = entity.GetComp<PlayerComponent>();
        CampComponent cc = entity.GetComp<CampComponent>();

        mainCamera = CameraService.Instance.m_mainCamera;

        m_entityID = entity.ID;

        GetEventKey(m_entityID);

        //ECSEvent.AddListener(m_InitKey, InitCharacter);
        //ECSEvent.AddListener(m_MoveKey, RefreshPoition);
        entity.World.eventSystem.AddListener(m_DamageKey, ECSBeAttack, true);
        entity.World.eventSystem.AddListener(m_RecoverKey, ECSBeRecover, true);
        //ECSEvent.AddListener(m_SKillKey, UseSkill);
        //ECSEvent.AddListener(m_AttackKey, UseSkill);
        //ECSEvent.AddListener(m_BeBreakKey, BeBreak);

        go_character = pc.perfab.gameObject;
        n_oldHp = lc.maxLife;

        //TODO 血条高度读取配置
        v3_FollowOffset.y = -0.15f * Screen.height;


        bloodIsFull = true;

        UpdateSliderLength(0, lc.maxLife);
        InitLifeSlider(cc.camp);
    }

    public void ECSBeAttack(EntityBase entity, params object[] pbjs)
    {
        //Debug.Log("ECSBeAttack " + entity.World.FrameCount);

        PerfabComponent pc = entity.GetComp<PerfabComponent>();
        LifeComponent lc = entity.GetComp<LifeComponent>();
        PlayerComponent plc = entity.GetComp<PlayerComponent>();
        CampComponent cc = entity.GetComp<CampComponent>();

        //Debug.Log("BeAttack -->");
        UpdateLifeSlider((float)lc.Life / (float)lc.maxLife);
        UpdateSliderLength(0, lc.maxLife);

        TextDisplaySizeType textSizeType = TextDisplaySizeType.Small;

        //if ((bool)args[1] == true)
        //{
        //    textSizeType = TextDisplaySizeType.Large;
        //}

        DisplayNum(n_oldHp - lc.Life , cc.camp, textSizeType, TextDisplayStyleType.Hurt);

        n_oldHp = lc.Life;
    }

    public void ECSBeRecover(EntityBase entity, params object[] pbjs)
    {
        PerfabComponent pc = entity.GetComp<PerfabComponent>();
        LifeComponent lc = entity.GetComp<LifeComponent>();
        PlayerComponent plc = entity.GetComp<PlayerComponent>();
        CampComponent cc = entity.GetComp<CampComponent>();

        UpdateLifeSliderRecover((float)lc.Life / (float)lc.maxLife);
        float recoverNum = lc.Life - n_oldHp;
        if (recoverNum > 0)
        {
            //if ((bool)(args[1]) == false || character.m_camp == Camp.Brave)//怪物的自动回血，不显示
            {
                DisplayNum(lc.Life - n_oldHp, Camp.Brave, TextDisplaySizeType.Middle, TextDisplayStyleType.Recover);
            }

        }
        n_oldHp = lc.Life;
    }

    #endregion

    #region 血条部分

    private bool UpdateShowOrNot()
    {
        

        if (bloodRoot == null)
        {
            return false;
        }

        //return true;

        if (bloodIsFull)
        {
            if (bloodRoot.enabled == true)
            {
                bloodRoot.enabled = false;
                return false;
            }
        }
        else
        {
            if (bloodRoot.enabled == false)
            {
                bloodRoot.enabled = true;
                return true;
            }
        }
        return true;
    }

    float m_newLifeRatio = 0;
    /// <summary>
    /// 更新血条（受伤时）
    /// </summary>
    /// <param name="新生命值的百分比"></param> 
    public void UpdateLifeSlider(float l_n_newLifeRatio)
    {
        //容错处理
        if (l_n_newLifeRatio<0)
        {
            l_n_newLifeRatio = 0;
        }

        bloodIsFull = l_n_newLifeRatio >= 1?true:false;

        m_newLifeRatio = l_n_newLifeRatio;
        //即时血条更新
        SetLifeSliderValue(l_n_newLifeRatio);

        //动画血条更新
        AnimSystem.CustomMethodToFloat(LifeSliderAni, n_oldLifeRatio, l_n_newLifeRatio, n_sliderAniTime, 0, InterpType.InCubic, callBack: UpdateSliderAnim);
 
    }

    void UpdateSliderAnim(object[] obj)
    {
        SetOldLifeRatio(m_newLifeRatio);
    }

    /// <summary>
    /// 更新血条（回复时）
    /// </summary>
    /// <param name="新生命值的百分比"></param> 
    public void UpdateLifeSliderRecover(float l_n_newLifeRatio)
    {
        //容错处理
        if (l_n_newLifeRatio < 0)
        {
            l_n_newLifeRatio = 0;
        }

        ////即时（暗色）血条更新
        //SetLifeSliderAniValue(l_n_newLifeRatio);

        AnimSystem.CustomMethodToFloat(LifeSliderAni, n_oldLifeRatio, l_n_newLifeRatio, n_sliderAniTime, 0, InterpType.InCubic);

        //动画血条更新
        AnimSystem.CustomMethodToFloat(LifeSlider, n_oldLifeRatio, l_n_newLifeRatio, n_sliderAniTime, 0, InterpType.InCubic, callBack: (o) =>
        {
            SetOldLifeRatio(l_n_newLifeRatio);
        });

    }


    /// <summary>
    /// 修改 及时更新（明亮）血条的长度
    /// </summary>
    /// <param name="l_n_ratio"></param>
    private void SetLifeSliderValue(float l_n_ratio)
    {
        lifeSlider.value = l_n_ratio;   

    }



    /// <summary>
    /// 修改 动画更新（暗色）血条的长度
    /// </summary>
    /// <param name="l_n_ratio"></param>
    private void SetLifeSliderAniValue(float l_n_ratio)
    {
        lifeAniSlider.value = l_n_ratio;
    }

  
    /// <summary>
    /// 修改（更新）上次的血量比例
    /// </summary>
    /// <param name="l_n_value"></param>
    private void SetOldLifeRatio(float l_n_value)
    {
        n_oldLifeRatio = l_n_value;
    }

    /// <summary>
    /// 动画更新数值(暗色条)
    /// </summary>
    private void LifeSliderAni(float l_n_aniValue)
    {
        SetLifeSliderAniValue(l_n_aniValue);
    }

    /// <summary>
    /// 动画更新数值（明亮条）
    /// </summary>
    private void LifeSlider(float l_n_aniValue)
    {
        SetLifeSliderValue(l_n_aniValue);
    }

    #endregion 

    #region 血条、盾条长度相关

    /// <summary>
    /// 修改血条背景的刻度以及血条、盾条的长度
    /// </summary>
    private void UpdateSliderLength(int nowShieldNum,int maxHP)
    {
        if (lifeSliderBG == null||ShieldSliderArea == null || nowShieldNum<0)
        {
            return;
        }

        float lifeScale = (float)maxHP / (float)n_standardLifeNum;
        float shieldScale = (float)nowShieldNum / (float)n_standardLifeNum;

        lifeSliderBG.sizeDelta = new Vector2((lifeScale + shieldScale) * n_standardLifeBGLength, lifeSliderBG.sizeDelta.y);
        lifeSliderBG.localScale = new Vector3(1 / (lifeScale + shieldScale), 1, 1);

        float totalNum = (float)maxHP + (float)nowShieldNum;
        float shieldRatio = (float)nowShieldNum / totalNum;
        float lifeRatio = (float)maxHP / totalNum;

        ShieldSliderArea.sizeDelta = new Vector2(n_standardLifeBGLength * shieldRatio, ShieldSliderArea.sizeDelta.y);
        lifeSliderArea.sizeDelta = new Vector2(n_standardLifeBGLength * lifeRatio, lifeSliderArea.sizeDelta.y);

    }
    #endregion

    #region 特殊状态进度条（现在只有技能前摇）

    public void BeginSpecialStatus(float time)
    {
        statusTimer = time;
        if (statusTimer > 0)
        {
            //Debug.Log("statusTimer" + statusTimer);
            statusSliderCanvas.enabled = true;
            statusTimerStep = 1 / statusTimer;
        }
        else
        {
            statusSliderCanvas.enabled = false;
            statusTimerStep = 0;
        }
        
    }

    //更新特殊进度条
    private void SpecialStatusChange()
    {
        if (statusSliderObj == null)
        {
            return;
        }
        if (statusTimer > 0)
        {
            statusTimer -= Time.deltaTime;
            statusSlider.value = 1 - statusTimer * statusTimerStep;
        }
        else if (statusSliderCanvas.enabled == true)
        {
            statusSliderCanvas.enabled = false;
        }

    }

    //被打断，停止进度
    private void SkillStatusStop()
    {
        if (statusSliderCanvas.enabled)
        {
            statusSliderCanvas.enabled = false;
        }
    }

    #endregion

    #region 数值显示部分

    /// <summary>
    /// 展示数字
    /// </summary>
    /// <param name="要展示的内容(float 或 string)"></param>
    /// <param name="内容大小类型"></param>
    /// <param name="内容样式类型"></param>
    public void DisplayNum(float num, Camp nowCamp = Camp.Brave,TextDisplaySizeType SizeType = TextDisplaySizeType.Small, TextDisplayStyleType StyleType = TextDisplayStyleType.Hurt)
    {
        int l_num = (int)num;
        if (l_num != 0)
        {
            DisplayNum((l_num).ToString(), nowCamp, SizeType, StyleType);
        }
        
    }

    private Vector3 disPlayTextPosition = new Vector3(30, 50, 0);

    public void DisplayNum(string text,Camp nowCamp, TextDisplaySizeType SizeType = TextDisplaySizeType.Small, TextDisplayStyleType StyleType = TextDisplayStyleType.Hurt)
    {
        PoolObject displayText = GameObjectManager.GetPoolObject("DisplayTextTemplate", tran_follow.gameObject);
        DisplayText dText = (DisplayText)displayText;
        GameObject go_displayText = displayText.gameObject;
        ModifySymble(dText, text, StyleType);
        //ModifyBySizeType(dText, SizeType);
        ModifyStyleType(dText, nowCamp,SizeType, StyleType);

        displayText.transform.localScale = Vector3.zero;
        displayText.transform.localPosition = disPlayTextPosition;

        dText.ShowAnim();
    }

    ///// <summary>
    ///// 根据内容大小类型，进行加工
    ///// </summary>
    ///// <param name="go_display"></param>
    ///// <param name="SizeType"></param>
    //private void ModifyBySizeType(DisplayText go_display, TextDisplaySizeType SizeType)
    //{

    //    switch (SizeType)
    //    {
    //        case TextDisplaySizeType.Small: go_display.ChangeFontSize( 75); break;
    //        case TextDisplaySizeType.Middle: go_display.ChangeFontSize(105); break;
    //        case TextDisplaySizeType.Large: go_display.ChangeFontSize(190); break;
    //        case TextDisplaySizeType.Critical: go_display.ChangeFontSize(315); break;
    //    }
 
    //}


    private void ModifySymble(DisplayText displayText, string text, TextDisplayStyleType StyleType)
    {
        if (StyleType == TextDisplayStyleType.Recover)
        {
            text = "+" + text;
        }
        else
        {
            text = "-" + text;
        }

        displayText.SetText(text);
    }

    /// <summary>
    /// 根据内容样式类型，进行加工
    /// </summary>
    /// <param name="go_display"></param>
    /// <param name="StyleType"></param>
    private void ModifyStyleType(DisplayText go_display, Camp nowCamp,TextDisplaySizeType SizeType, TextDisplayStyleType StyleType)
    {

        if (StyleType == TextDisplayStyleType.Recover)
        {
            go_display.ChangeFontSize(10);
            go_display.ChangeFont(FontType.Recover);
        }
        else
        {
            if (nowCamp == Camp.Brave)
            {
                go_display.ChangeFont(FontType.Brave);
                switch (SizeType)
                {
                    case TextDisplaySizeType.Small: go_display.ChangeFontSize(10); break;
                    case TextDisplaySizeType.Middle: go_display.ChangeFontSize(15); break;
                    case TextDisplaySizeType.Large: go_display.ChangeFontSize(20); break;
                    case TextDisplaySizeType.Critical: go_display.ChangeFontSize(32); break;
                }
            }
            else
            {
                switch (SizeType)
                {
                    case TextDisplaySizeType.Small: go_display.ChangeFontSize(10); go_display.ChangeFont(FontType.Small); break;
                    case TextDisplaySizeType.Middle: go_display.ChangeFontSize(15); go_display.ChangeFont(FontType.Middle); break;
                    case TextDisplaySizeType.Large: go_display.ChangeFontSize(20); go_display.ChangeFont(FontType.Large); break;
                    case TextDisplaySizeType.Critical: go_display.ChangeFontSize(32); go_display.ChangeFont(FontType.Large); break;
                }
            }
 
        }
        


        //switch (StyleType)
        //{
        //    case TextDisplayStyleType.Hurt: go_display.ChangeFontColor(Color.red, new Color(0,0,0,0.5f)); break;
        //    case TextDisplayStyleType.Recover: go_display.ChangeFontColor(Color.green, Color.yellow); break;
        //}
    }

    //字体大小类型
    public enum TextDisplaySizeType
    {
        Small,
        Middle,
        Large,
        Critical
    }

    //字体样式类型
    public enum TextDisplayStyleType
    {
        Hurt,
        Recover
    }

    #endregion

    #region UI跟随

    Vector3 l_v3_tarPos;        //跟随角色的位置
    Vector2 l_v2_ScreenPos;
    Vector3 l_v3_ScreenPos;

    //跟随偏移
    public Vector3 v3_FollowOffset = Vector3.zero;

    private Vector3 v3_heightOffset = new Vector3(0,0,0);

    //以1920*1080为基础，以width为基准的ui缩放比
    float screenScale;

    //UI位置跟随
    private void UIFollow()
    {
        //Debug.Log(go_character + " camera " + mainCamera);
        if (go_character == null || mainCamera == null)
            return;

        if (bloodIsFull && statusTimer <= 0)  //满血，并且没有吟唱，不用跟随，节省资源
        {
            return;
        }



        l_v3_tarPos = go_character.transform.position + v3_heightOffset;
        l_v3_ScreenPos = mainCamera.WorldToViewportPoint(l_v3_tarPos);

        Vector3 newPos = new Vector3((l_v3_ScreenPos.x - 0.5f) * Screen.width * screenScale, (l_v3_ScreenPos.y - 0.5f) * Screen.height * screenScale, 0);
        tran_follow.localPosition = newPos + v3_FollowOffset;
    }

    #endregion

}