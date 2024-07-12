using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 角色基础类
/// </summary>
[RequireComponent(typeof(MoveCompmoent))]
[RequireComponent(typeof(AnimCompmoent))]
[RequireComponent(typeof(EffectCompmoent))]
[RequireComponent(typeof(CharacterUICompmoent))]
[RequireComponent(typeof(CharacterMaterialCompmoent))]
[RequireComponent(typeof(AudioCompmoent))]
public abstract class CharacterBase : PoolObject
{
    #region 基础属性

    public CharacterBaseProperty m_Property;
    public string m_characterName;
    public int m_characterID;
    public Transform m_waistNode;


    private bool m_isAlive = true;
    public bool IsAlive
    {
        get { return m_isAlive; }
        set { m_isAlive = value; }
    }

    public Camp m_camp;

    public bool m_isCloaking   = false; //隐身
    public bool m_isInvincible = false; //无敌
    public bool m_isTakeOver   = false; //Buff接管
    public bool m_isTrueSight  = false; //真视

    #endregion

    #region 基础数据

    public CharacterHardPoint m_hardPoint;
    public GameObject m_aimCircle;

    public List<SkillData> m_attackInfoList; //普攻列表
    public List<SkillData> m_skilInfoList;   //技能列表
    public List<BuffBase> m_buffList = new List<BuffBase>();        //Buff列表

    #endregion

    #region 组件

    public MoveCompmoent m_moveComp;
    public AnimCompmoent m_animComp;
    public EffectCompmoent m_effectComp;
    public CharacterUICompmoent m_UIComp;
    public CharacterMaterialCompmoent m_materialComp;
    public AudioCompmoent m_audio;
    public SkinnedMeshRenderer[] m_skinnedMeshRenderer;
    


    #endregion

    #region 状态

    public MoveStatus m_moveStatus;
    public AttackSatus m_atkStatus;
    public SkillStatus m_skillStatus;
    public DieStatus m_dieStatus;
    public HurtStatus m_hurtStatus;
    public BlowFlyStatus m_blowFlyStatus;
    public BuffStatus m_buffStatus;

    #endregion

    #region 行为管理

    public CharacterStatusEnum m_currentStatusType;
    public CharacterBaseStatus m_currentStatus;
    public HurtModel m_hurtModle;

    Dictionary<int, CharacterBaseStatus> m_status = new Dictionary<int, CharacterBaseStatus>();

    /// <summary>
    /// 角色当前状态
    /// </summary>
    public CharacterBaseStatus CurrentStatus
    {
        get { return m_currentStatus; }
        set { m_currentStatus = value; }
    }

    #endregion

    #region 方法

    #region 编辑器拓展方法

    [ContextMenu("Find hard Points For Player")]
    public void FindHardPointsPlayer()
    {
        m_hardPoint.m_head = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head");
        m_hardPoint.m_hand_L = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
        m_hardPoint.m_hand_R = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
        m_hardPoint.m_chest = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine");

        m_hardPoint.m_weapon01 = transform.Find("Bip01/Bip01 Prop1");
        m_hardPoint.m_weapon02 = transform.Find("Bip01/Bip01 Prop2");

        m_waistNode = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine1");

        m_hardPoint.m_pos = transform;
        FindHeadTopPoint();
    }


    [ContextMenu("Find headTop Point")]
    public void FindHeadTopPoint()
    {
        if (m_hardPoint.m_head == null)
        {
            Debug.Log(transform.name + " 的head 为空！");
            return;
        }
        m_hardPoint.m_headTop = transform.Find("headTop");
        if (m_hardPoint.m_headTop == null)
        {
            GameObject headTop = new GameObject();
            headTop.name = "headTop";
            m_hardPoint.m_headTop = headTop.transform;
            headTop.transform.parent = transform;
           
            
        }
        m_hardPoint.m_headTop.transform.position = m_hardPoint.m_head.position + Vector3.up * 0.5f;


    }

    [ContextMenu("Find hard Points For Monster")]
    public void FindHardPointsMonster()
    {
        m_hardPoint.m_pos = transform;
        FindHardPointsChild(transform);
        FindHeadTopPoint();
    }

    [ContextMenu("Find hard Points For Trap")]
    public void FindHardPointsTrap()
    {
        m_hardPoint.m_head = transform;
        m_hardPoint.m_hand_L = transform;
        m_hardPoint.m_hand_R = transform;
        m_hardPoint.m_chest = transform;

        m_hardPoint.m_weapon01 = transform;
        m_hardPoint.m_weapon02 = transform;
        m_hardPoint.m_pos = transform;
        m_hardPoint.m_headTop = transform;
        //FindHeadTopPoint();
    }


    void FindHardPointsChild(Transform tran )
    {
        foreach(var child in tran.GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("L Hand"))
            {
                m_hardPoint.m_hand_L = child;
            }
            if (child.name.Contains("R Hand"))
            {
                m_hardPoint.m_hand_R = child;
            }
            if (child.name.Contains("Head"))
            {
                m_hardPoint.m_head = child;
            }
            if (child.name.Contains(" Spine"))
            {
                m_hardPoint.m_chest = child;
            }
        }
 
    }

    #endregion

    #region 生命周期事件

    void Reset()
    {
    }

    public override void OnCreate()
    {
        m_moveComp = GetComponent<MoveCompmoent>();
        m_animComp = GetComponent<AnimCompmoent>();
        m_effectComp = GetComponent<EffectCompmoent>();
        m_UIComp = GetComponent<CharacterUICompmoent>();
        m_materialComp = GetComponent<CharacterMaterialCompmoent>();
        m_audio = GetComponent<AudioCompmoent>();
        m_skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();

        try
        {
            m_materialComp.OnCreate();
            m_moveComp.OnCreate();
            m_animComp.OnCreate();
            m_effectComp.OnCreate();
            m_audio.OnCreate();

            InitAimChircle();

            //创建时回调
            OnCharacterCreate();
        }
        catch (Exception e)
        {
            Debug.LogError("->" + gameObject.name +  "<- OnCreate Error:" + e.ToString());
        }
    }

    public virtual void OnDie()
    {
        CleanBuff();

        m_UIComp.OnDie();
        m_materialComp.OnDie();
        m_moveComp.OnDie();
        m_animComp.OnDie();
        m_effectComp.OnDie();

        RecycleBody();
    }

    public void Init(string characterName,int characterID)
    {
        m_characterID = characterID;
        m_characterName = characterName;

        m_moveComp.OnInit();
        m_animComp.OnInit();
        m_effectComp.OnInit();
        m_effectComp.OnInit();

        try
        {
            //创建时回调
            OnInit(characterName, characterID);
        }
        catch(Exception e)
        {
            Debug.LogError("->" + characterName + "<- ID: " + characterID + " OnInit Exception " + e.ToString());
        }

        m_UIComp.OnCreate();

        MyReset();

        CharacterManager.Dispatch(m_characterID, CharacterEventType.Init, this);
    }


    public void Dissolve()
    {
        Material m_material = null;
        for (int i = 0; i < m_skinnedMeshRenderer.Length;i++ )
        {
            if (m_skinnedMeshRenderer[i] != null)
            {
                m_material = m_skinnedMeshRenderer[i].sharedMaterial;
            }
            else
            {
                Debug.Log(transform.name + "子节点下 没有m_skinnedMeshRenderer");
            }

            if (m_material != null)
            {
                bool changed = false;
                MaterialManager.ChangeMeterial(m_skinnedMeshRenderer[i].gameObject, "Dissolve", ref changed, "_MainTex");

                m_skinnedMeshRenderer[i].sharedMaterial.SetFloat("_NowTime", Time.timeSinceLevelLoad);
            }
        }
    }

    public override void OnObjectDestroy()
    {
        base.OnObjectDestroy();
    }

    public void Destroy()
    {
        ExitCurrentStatus();

        m_UIComp.Destroy();
        m_materialComp.Destroy();
        m_moveComp.Destroy();
        m_animComp.Destroy();
        m_effectComp.Destroy();
        m_audio.Destroy();
    }

    #region 重写方法

    //在这里不同的子类加载不同位置的数据
    public virtual void OnCharacterCreate()
    {
        //加载攻击数据

        //加载技能数据
    }


    public virtual void OnInit(string characterName, int characterID)
    {
        //加载攻击数据

        //加载技能数据
    }

    public virtual void OnResurgence()
    {
        MyReset();
        m_UIComp.OnResurgence();
        m_materialComp.OnResurgence();
        m_moveComp.OnResurgence();
        m_animComp.OnResurgence();
        m_effectComp.OnResurgence();

        CharacterManager.Dispatch(m_characterID, CharacterEventType.Recover, this); //通知数值显示
    }

    public virtual void MyReset()
    {
        CleanBuff();

        m_isAlive = true;
        //m_recoverTimer = 0;
        ChangeStatus(CharacterStatusEnum.Move);

        m_animComp.ChangeAnim(m_Property.m_idleAnimName,0.5f);
        m_Property.ResetProperty();
    }

    protected virtual void Update()
    {
        if(IsAlive)
        {
            UpdateBuff();
            UpdateRecoverHp();
        }

        if (m_currentStatus != null)
        {
            m_currentStatus.OnUpdate();

            if (m_currentStatus.CanMove())
            {
                m_moveStatus.OnUpdate();
            }
        }

        if (m_hurtModle != null)
        {
            m_hurtModle.Update();
        }
    }

    //回收角色
    protected virtual void RecycleBody()
    {
        Timer.DelayCallBack(3f, (objs) => {

            if (CharacterManager.GetCharacterIsExit(m_characterID))
            {
                ReDissolve();
                CharacterManager.DestroyCharacter(m_characterID); 
            }
        });
    }

    #endregion

    #endregion

    #region 接受指令

    public virtual void Attack(AttackCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            if (m_atkStatus.IsCurrentStatus)
            {
                m_atkStatus.ReceviceCmd(cmd);
            }
            else
            {
                if (m_currentStatus.CanSwitchOtherStatus(CharacterStatusEnum.Attack))
                {
                    ChangeStatus(CharacterStatusEnum.Attack);
                    m_atkStatus.ReceviceCmd(cmd);
                }
            }
        }
    }

    public virtual void Skill(SkillCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            if (m_skillStatus.IsCurrentStatus)
            {
                m_skillStatus.ReceviceCmd(cmd);
            }
            else
            {
                if (m_currentStatus.CanSwitchOtherStatus(CharacterStatusEnum.Skill))
                {
                    ChangeStatus(CharacterStatusEnum.Skill);
                    m_skillStatus.ReceviceCmd(cmd);
                }
            }
        }
    }



    public virtual void Move(MoveCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            MoveStatus ms = m_moveStatus;

            if (ms.IsCurrentStatus)
            {
                ms.Move(cmd);
            }
            else
            {
                if (m_currentStatus.CanSwitchOtherStatus(CharacterStatusEnum.Move))
                {
                    CharacterManager.Dispatch(m_characterID, CharacterEventType.Move, this);
                    ChangeStatus(CharacterStatusEnum.Move);
                    ms.Move(cmd);
                }
                else
                {
 
                    if (m_currentStatus.CanMove())
                    {
                        ms.Move(cmd);
                    }
                }
            }
        }
    }

    public virtual void Rotation(RotationCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            MoveStatus ms = m_moveStatus;
            if (ms.IsCurrentStatus)
            {
                ms.Rotation(cmd);
            }
            else
            {
                if (m_currentStatus.CanSwitchOtherStatus(CharacterStatusEnum.Move))
                {
                    CharacterManager.Dispatch(m_characterID, CharacterEventType.Move, this);
                    ChangeStatus(CharacterStatusEnum.Move);
                    ms.Rotation(cmd);
                }
                else
                {

                    if (m_currentStatus.CanMove())
                    {
                        ms.Rotation(cmd);
                    }
                }
            }
        }
    }

    public virtual void Die(DieCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            ChangeStatus(CharacterStatusEnum.Die);
            OnDie();

            //DisplayManager.DropOut(cmd.m_killerID, cmd.m_characterID);
            CharacterManager.Dispatch(m_characterID, CharacterEventType.Die, this, cmd.m_killerID);
        }
    }

    public virtual void Resurgence(ResurgenceCmd cmd)
    {
        //Debug.Log("Resurgence ");
        ChangeStatus(CharacterStatusEnum.Move);
        OnResurgence();

        //AddBuff("ResurgenceBuff",null,-1);

        ReDissolve();
        CharacterManager.Dispatch(m_characterID, CharacterEventType.Resurgence, this );
    }

    public virtual void BeDamage(DamageCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            //m_recoverTimer = 0;
            m_hurtModle.BeAttack(cmd);
            CharacterManager.Dispatch(m_characterID, CharacterEventType.Damage, this, cmd.m_attackerID, cmd.m_Crit); //通知数值显示
        }
    }

    public virtual void BeRecover(RecoverCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            //m_hurtModle.BeAttack(cmd);
            m_hurtModle.BeRecover(cmd);
            CharacterManager.Dispatch(m_characterID, CharacterEventType.Recover, this, cmd.m_attackerID, cmd.m_isAutoRecover); //通知数值显示
        }
    }

    public virtual void Blowfly(BlowFlyCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
        {
            ChangeStatus(CharacterStatusEnum.Blowfly);
            m_blowFlyStatus.BlowFly(cmd);
        }
    }

    public virtual void AddBuff(AddBuffCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
            AddBuff(cmd.m_buffID,cmd.m_skillID,cmd.m_attackerID);
    }

    public virtual void RemoveBuff(RemoveBuffCmd cmd)
    {
        if (m_isAlive && m_currentStatus != null)
            RemoveBuff(cmd.m_buffID);
    }

    //public virtual void TrapDamage(TrapDamageCmd cmd)
    //{
    //    m_hurtModle.TrapDamage(cmd);
    //    CharacterManager.Dispatch(m_characterID, CharacterEventType.Damage, this); //通知数值显示
    //}

    #endregion 

    #region 获取数据

    Area m_BodyArea;

    /// <summary>
    /// 获取占地大小
    /// </summary>
    /// <returns></returns>
    public virtual Area GetBodyArea()
    {
        if (m_BodyArea == null)
        {
            if (m_Property == null)
            {
                Debug.LogError(m_characterName + " m_Property is null");
            }

            m_BodyArea = new Area();
            m_BodyArea.areaType = AreaType.Circle;
            m_BodyArea.radius = m_Property.m_radius;
        }

        m_BodyArea.direction = transform.forward;
        m_BodyArea.position  = transform.position;

        return m_BodyArea;
    }

    Area m_visable;
    public virtual Area GetVisableArea()
    {
        if (m_visable == null)
        {
            m_visable = new Area();
            m_visable.areaType = AreaType.Circle;
            m_visable.radius = 5;
        }

        m_visable.position = transform.position;
        m_visable.direction = transform.forward;

        return m_visable;
    }

    public void SetVisableArea(Area area)
    {
        //Debug.Log("SetVisableArea(Area area)");

        area.position = transform.position;
        area.direction = transform.forward;

        m_visable = area;

        if (m_VisibleTip != null)
        {
            m_VisibleTip.SetArea(m_visable,false,null);
        }
    }

    public void SetVisableArea(string areaID)
    {
        //Debug.Log("SetVisableArea(string areaID)");

        if (m_visable == null)
        {
            m_visable = new Area();
        }

        m_visable.UpdateArea(areaID, this);

        if (m_VisibleTip != null)
        {
            m_VisibleTip.SetArea(m_visable, false, null);
        }
    }


    Area m_AttackDamageArea = new Area();
    public virtual Area GetAttackDamageArea(string skillID,CharacterBase skiller,CharacterBase aim = null)
    {
        m_AttackDamageArea.UpdatSkillArea(skillID, skiller, aim);
        return m_AttackDamageArea;
    }

    /// <summary>
    /// 获取普通攻击ID
    /// </summary>
    /// <param name="l_n_index"></param>
    /// <returns></returns>
    public SkillData GetAttackData(int attackIndex)
    {
        if (attackIndex >= m_attackInfoList.Count)
        {
            throw new Exception("DONT FIND AttackInfo index:" + attackIndex + " by " + m_characterName);
        }
        else
        {
            return m_attackInfoList[attackIndex];
        }
    }

    public SkillData GetAttackData(string attackID)
    {
        for (int i = 0; i < m_attackInfoList.Count; i++)
        {
            if (m_attackInfoList[i].m_skillID == attackID)
            {
                return m_attackInfoList[i];
            }
        }

        throw new Exception("DONT FIND AttackData attackID:" + attackID + " by " + m_characterName);
    }

    public SkillData GetSkillData(int skillIndex)
    {
        if (skillIndex >= m_skilInfoList.Count)
        {
            throw new Exception("DONT FIND SkillInfo index:" + skillIndex + " by " + m_characterName);
        }
        else
        {
            return m_skilInfoList[skillIndex];
        }
    }

    public SkillData GetSkillData(string skillID)
    {
        for (int i = 0; i < m_skilInfoList.Count; i++)
        {
            if (m_skilInfoList[i].m_skillID == skillID)
            {
                return m_skilInfoList[i];
            }
            
        }

        throw new Exception("DONT FIND SkillInfo skillID:" + skillID + " by " + m_characterName);
    }

    public virtual float GetSpeed()
    {
        float speed = m_Property.Speed;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_SpeedChange;
                changePercantage *= m_buffList[i].m_buffData.m_SpeedChangePercentage;
            }

            speed *= changePercantage;
            speed += changeNumber;
        }

        //Debug.Log("speed " + speed);

        return speed;
    }

    public virtual int GetAttack()
    {
        float attack = (float)m_Property.m_Attack;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_AttackChange;
                changePercantage *= m_buffList[i].m_buffData.m_AttackChangePercantage;
            }

            attack *= changePercantage;
            attack += changeNumber;
        }

        return (int)attack;
    }

    public virtual int GetDefense()
    {
        float defense = (float)m_Property.m_Defense;

        //Debug.Log("原始防御 ：" + defense);

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_defChange;

                //Debug.Log("防御改变 " + m_buffList[i].m_buffData.m_defChange);

                changePercantage *= m_buffList[i].m_buffData.m_AttackChangePercantage;
            }

            defense *= changePercantage;
            defense += changeNumber;
        }

        defense = Mathf.Max(0, defense);

        //Debug.Log("最终防御 ：" + (int)defense);

        return (int)defense;
    }

    public virtual int GetCrit()
    {
        float crit = (float)m_Property.m_Crit;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_critChange;
                changePercantage *= m_buffList[i].m_buffData.m_AttackChangePercantage;
            }

            crit *= changePercantage;
            crit += changeNumber;
        }

        return (int)crit;
    }

    public virtual int GetCritDamage()
    {
        float critDamage = (float)m_Property.m_CritDamage;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_critdamageChange;
                changePercantage *= m_buffList[i].m_buffData.m_AttackChangePercantage;
            }

            critDamage *= changePercantage;
            critDamage += changeNumber;
        }

        return (int)critDamage;
    }

    public virtual int GetTough()
    {
        float tough = (float)m_Property.m_CritDamage;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_toughChange;
                changePercantage *= m_buffList[i].m_buffData.m_AttackChangePercantage;
            }

            tough *= changePercantage;
            tough += changeNumber;
        }

        return (int)tough;
    }

    public virtual int GetHpRecover()
    {
        float HpRecover = (float)m_Property.m_HpRecover;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_hprecoverChange;
                changePercantage *= m_buffList[i].m_buffData.m_AttackChangePercantage;
            }

            HpRecover *= changePercantage;
            HpRecover += changeNumber;
        }

        return (int)HpRecover;
    }

    public virtual int GetHpabsorb()
    {
        float Hpabsorb = (float)m_Property.m_HpAbsorb;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_hpabsorbChange;
                changePercantage *= m_buffList[i].m_buffData.m_hpabsorbPercantage;
            }

            Hpabsorb *= changePercantage;
            Hpabsorb += changeNumber;
        }

        return (int)Hpabsorb;
    }

    public virtual int GetIgnoreDefense()
    {
        float ignoreDefense = (float)m_Property.m_IgnoreDefense;

        if (m_buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < m_buffList.Count; i++)
            {
                changeNumber += m_buffList[i].m_buffData.m_ignoredefChange;
                changePercantage *= m_buffList[i].m_buffData.m_ignoredefPercantage;
            }

            ignoreDefense *= changePercantage;
            ignoreDefense += changeNumber;
        }

        return (int)ignoreDefense;
    }

    public virtual bool GetInvincible()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_Invincible)
            {
                m_isInvincible = true;
                return true;
            }
        }
        m_isInvincible = false;
        return false;
    }


    public bool GetCloaking()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_Cloaking)
            {
                m_isCloaking = true;
                return true;
            }
        }
        m_isCloaking = false;
        return false;
    }

    public bool GetTrueSight()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_TrueSight)
            {
                m_isTrueSight = true;
                return true;
            }
        }

        m_isTrueSight = false;
        return false;
    }

    public bool GetTakeOver()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_IsTakeOver)
            {
                m_isTakeOver = true;
                return true;
            }
        }

        m_isTakeOver = false;
        return false;
    }

    public VisibleEnum GetVisible(Camp camp, bool isTrueSight)
    {
        if (!GetCloaking())
        {
            return VisibleEnum.Visible;
        }
        else
        {
            if (m_camp == camp)
            {
                return VisibleEnum.CloakingVisible;
            }
            else
            {
                if (isTrueSight)
                {
                    return VisibleEnum.CloakingVisible;
                }
                else
                {
                    return VisibleEnum.inVisible;
                }
            }
        }
    }


    #endregion

    #region 功能函数

    public void AimToCamera()
    {
        if (CameraService.Instance.m_mainCameraGo != null)
        {
            transform.LookAt(CameraService.Instance.m_mainCameraGo.transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        } 
    }

    /// <summary>
    /// 重设可见性
    /// </summary>
    public void ResetVisible()
    {
        if (GameLogic.GetMyPlayerCharacter() != null)
        {
            m_materialComp.SetVisible(GetVisible(GameLogic.myCamp, GameLogic.GetMyPlayerCharacter().GetTrueSight()));
        }
        else
        {
            m_materialComp.SetVisible(GetVisible(GameLogic.myCamp, false));
        }
    }

    public void ResetTrueSight()
    {
        GetTrueSight();

        if(m_characterID == GameLogic.MyPlayerID)
        {
            //重设周围角色可见性
            List<CharacterBase> list = CharacterManager.CharacterList;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].ResetVisible();
            }
        }
    }

    /// <summary>
    /// 重设接管
    /// </summary>
    public void ResetTakeOver()
    {
        if(GetTakeOver())
        {
            ChangeStatus(CharacterStatusEnum.Buff);
        }
        else
        {
            ChangeStatus(CharacterStatusEnum.Move);
        }
    }
    CreatMesh m_VisibleTip;
    public void SetVisibleTip(bool isShow,bool isEnemy)
    {
        if (m_VisibleTip == null)
        {
            GameObject tip = GameObjectManager.CreateGameObjectByPool("AreaTips");
            m_VisibleTip = tip.GetComponent<CreatMesh>();

            tip.transform.SetParent(transform);

            tip.transform.localPosition = new Vector3(0, 0.2f, 0);
            tip.transform.localEulerAngles = new Vector3(0, -90, 0);

            m_VisibleTip.SetArea(GetVisableArea(), isEnemy, null);
        }

        if (m_VisibleTip.gameObject.activeSelf != isShow)
        {
            m_VisibleTip.gameObject.SetActive(isShow);
        }

        m_VisibleTip.SetEnemy(isEnemy);
        
    }

    //初始化目标指示器
    private void InitAimChircle()
    {
        //if (m_aimCircle != null)
        //{
        //    return;
        //}
        //string c_aimCircleName =  DataGenerateManager<ConstantDataGenerate>.GetData("aimCircleName").m_StringValue;
        //Transform aimCircleTran = transform.FindChild(c_aimCircleName);
        
        //m_aimCircle = aimCircleTran == null? null: aimCircleTran.gameObject;
        //if (m_aimCircle == null)
        //{
        //    string aimCircleFXName = DataGenerateManager<ConstantDataGenerate>.GetData("aimCircleFXName").m_StringValue;
        //    m_aimCircle = GameObjectManager.CreateGameObjectByPool(aimCircleFXName, gameObject);
        //    m_aimCircle.transform.localPosition = Vector3.zero;
        //}
        //HideAimChircle();
    }

    //显示目标指示器
    public void ShowAimChircle()
    {
        //m_aimCircle.SetActive(true);
    }

    //隐藏目标指示器
    public void HideAimChircle()
    {
        //m_aimCircle.SetActive(false);
    }

    //float m_recoverTimer = 0;
    void UpdateRecoverHp()
    {
        //if (m_Property != null)
        //{
        //    m_recoverTimer += Time.deltaTime;

        //    if (m_recoverTimer > 1)
        //    {
        //        m_recoverTimer = 0;
        //        //
        //    }
        //}
    }

    //重置消融状态
    void ReDissolve()
    {
        for (int i = 0; i < m_skinnedMeshRenderer.Length; i++)
        {
            MaterialManager.Reduction(m_skinnedMeshRenderer[i].gameObject);
        }
    }


    public void MaterialHighLight()
    {
        for(int i= 0;i<m_skinnedMeshRenderer.Length;i++)
        {
            m_skinnedMeshRenderer[i].material.SetFloat("_NowTime", Time.timeSinceLevelLoad);
        }
    }


    #endregion

    #region 状态管理

    public CharacterBaseStatus GetStatus(CharacterStatusEnum statusType)
    {
        if (m_status.ContainsKey(statusType.GetHashCode()))
        {
            return m_status[statusType.GetHashCode()];
        }
        else
        {
            throw new Exception(statusType + " not exits!");
        }
    }

    //public T GetStatus<T>(CharacterStatusEnum statusType) where T : CharacterBaseStatus
    //{
    //    if (m_status.ContainsKey(statusType.GetHashCode()))
    //    {
    //        try
    //        {
    //            return (T)m_status[statusType.GetHashCode()];
    //        }
    //        catch(Exception e)
    //        {
    //            throw new Exception(statusType + "/n" + e.ToString());
    //        }
    //    }
    //    else
    //    {
    //        throw new Exception(statusType + " not exits!");
    //    }
    //}

    public void ChangeStatus(CharacterStatusEnum statusType)
    {
        if (m_currentStatus != null)
        {
            m_currentStatus.IsCurrentStatus = false;
            m_currentStatus.OnExitStatus();
        }

        m_currentStatusType = statusType;
        CharacterBaseStatus newStatus = GetStatus(statusType);

        newStatus.IsCurrentStatus = true;
        newStatus.OnEnterStatus();

        
        m_currentStatus = newStatus;
    }

    public CharacterBaseStatus LoadStatusInterface<T>(CharacterStatusEnum statusType) where T : CharacterBaseStatus, new()
    {
        if (m_status.ContainsKey(statusType.GetHashCode()))
        {
            return m_status[statusType.GetHashCode()];
        }
        else
        {
            T status = new T();
            status.Init(this);
            m_status.Add(statusType.GetHashCode(), status);
            return status;
        }
    }

    public void LoadHurtModel<T>() where T : HurtModel, new()
    {
        m_hurtModle = new T();
        m_hurtModle.Init(this);
    }

    public void ExitCurrentStatus()
    {
        if (m_currentStatus != null)
        {
            m_currentStatus.IsCurrentStatus = false;
            m_currentStatus.OnExitStatus();
            m_currentStatus = null;
        }
    }

    #endregion 

    #region Buff

    public void AddBuff(string buffID,string skillID,int createrID)
    {
        BuffBase buff = GetBuff(buffID);

        if (buff == null)
        {
            buff = new BuffBase();
            buff.Init(this, createrID, buffID, skillID);
            m_buffList.Add(buff);

            buff.StartBuff();
        }
        else
        {
            //叠加Buff
            buff.ResetBuff();
        }
    }

    public void RemoveBuff(string buffID)
    {
        BuffBase buff = GetBuff(buffID);
        
        if(buff != null)
        {
            RemoveBuff(buff);
        }
    }

    public void RemoveBuff(BuffBase buff)
    {
        m_buffList.Remove(buff);
        buff.EndBuff();
    }

    void UpdateBuff()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            m_buffList[i].Update();

            if (m_buffList[i].isFinsih)
            {
                RemoveBuff(m_buffList[i]);
                i--;
            }
        }
    }

    void CleanBuff()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            m_buffList[i].EndBuff();
        }
        m_buffList.Clear();
    }

    BuffBase GetBuff(string buffID)
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_key == buffID)
            {
                return m_buffList[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 使用技能打断Buff
    /// </summary>
    public void UseSkillInterruptBuff()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_UseSkilIinterrupt)
            {
                BuffBase buff = m_buffList[i];
                m_buffList.RemoveAt(i);

                buff.EndBuff();

                i--;
            }
        }
    }

    /// <summary>
    /// 受到伤害打断Buff
    /// </summary>
    public void BeDamageInterruptBuff()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_BeDamageInterrupt)
            {
                BuffBase buff = m_buffList[i];
                m_buffList.RemoveAt(i);

                buff.EndBuff();
                i--;
            }
        }
    }

    /// <summary>
    /// 打断隐身Buff
    /// </summary>
    public void InterruptCloakingBuff()
    {
        for (int i = 0; i < m_buffList.Count; i++)
        {
            if (m_buffList[i].m_buffData.m_Cloaking)
            {
                BuffBase buff = m_buffList[i];
                m_buffList.RemoveAt(i);

                buff.EndBuff();
                i--;
            }
        }
    }

    #endregion

    #endregion

}
public enum CharacterStatusEnum
{
    Move,
    Attack,
    Skill,
    Hurt,
    Die,
    Blowfly, //击飞
    Buff,    //buff接管，眩晕，恐惧，嘲讽等
}

[System.Serializable]
public class CharacterHardPoint
{
    public Transform m_head;
    public Transform m_hand_R;
    public Transform m_hand_L;
    public Transform m_chest;
    public Transform m_pos;
    public Transform m_weapon01;
    public Transform m_weapon02;
    public Transform m_enemy;
    public Transform m_headTop;

    public Transform GetHardPoint(HardPointEnum pointEnum)
    {
        Transform result = null;

        switch (pointEnum)
        {
            case HardPointEnum.head: result = m_head;break;
            case HardPointEnum.chest: result = m_chest;break;
            case HardPointEnum.hand_R: result = m_hand_R;break;
            case HardPointEnum.hand_L: result = m_hand_L;break;
            case HardPointEnum.position: result = m_pos;break;
            case HardPointEnum.enemy: result = m_enemy;break;
            case HardPointEnum.Weapon_01: result = m_weapon01; break;
            case HardPointEnum.Weapon_02: result = m_weapon02; break;
            case HardPointEnum.headTop: result = m_headTop; break; 
            default: throw new Exception("ERROR HardPointEnum " + pointEnum );
        }

        if (result == null)
        {
            throw new Exception("ERROR HardPoint is null ! " + pointEnum);
        }

        return result;
    }
}

public enum HardPointEnum
{
    head,
    hand_R,
    hand_L,
    chest,
    position, //当前位置，无坐标
    enemy,    //敌人位置处
    Weapon_01,
    Weapon_02,
    headTop, //头上方
}

public enum VisibleEnum
{
    Visible,         //完全可见
    inVisible,       //不可见
    CloakingVisible, //潜行可见
}