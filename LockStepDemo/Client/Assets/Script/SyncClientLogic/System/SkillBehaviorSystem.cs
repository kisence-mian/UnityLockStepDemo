using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SkillBehaviorSystem : SystemBase
{
    public override Type[] GetFilter()
    {
        return new Type[] {
            typeof(SkillStatusComponent),
        };
    }

    public override void Init()
    {
        m_world.eventSystem.AddListener(GameUtils.c_SkillHit, ReceviceSkillHit);
        m_world.eventSystem.AddListener(GameUtils.c_SkillStatusEnter, ReceviceSkillEnter);
    }

    public override void Dispose()
    {
        m_world.eventSystem.RemoveListener(GameUtils.c_SkillHit, ReceviceSkillHit);
        m_world.eventSystem.RemoveListener(GameUtils.c_SkillStatusEnter, ReceviceSkillEnter);
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            AddComp(list[i]);
            SkillBehaviorLogic(list[i], deltaTime);
        }
    }

    void AddComp(EntityBase entity)
    {
        if(!entity.GetExistComp<SkillBehaviorCompoent>())
        {
            entity.AddComp<SkillBehaviorCompoent>();
        }
    }

    void SkillBehaviorLogic(EntityBase entity,int deltaTime)
    {
        SkillBehaviorCompoent sbc = entity.GetComp<SkillBehaviorCompoent>();
        SkillStatusComponent ssc = entity.GetComp<SkillStatusComponent>();

        if(ssc.m_skillStstus == SkillStatusEnum.Before ||
            ssc.m_skillStstus == SkillStatusEnum.Current ||
            ssc.m_skillStstus == SkillStatusEnum.Later)
        {
            sbc.FXTimer += deltaTime;

            if (!sbc.isTriggerFX && sbc.FXTimer > GetDelayTime(ssc))
            {
                sbc.isTriggerFX = true;
                CreatSkillEffect(entity, ssc);
            }
        }
    }

    void ReceviceSkillHit(EntityBase entity,params object[] objs)
    {
        SkillStatusComponent sc = entity.GetComp<SkillStatusComponent>();
        CreateSkillAreaTip(entity, sc);
    }

    void ReceviceSkillEnter(EntityBase entity, params object[] objs)
    {
        AddComp(entity);
        SkillBehaviorCompoent sbc = entity.GetComp<SkillBehaviorCompoent>();

        sbc.FXTimer = 0;
        sbc.isTriggerFX = false;
    }

    CreatMesh RangeTip;


    public void CreatSkillEffect(EntityBase entity,SkillStatusComponent skillComp)
    {
        SkillStatusDataGenerate skillStatusData = null;
        switch (skillComp.m_skillStstus)
        {
            case SkillStatusEnum.Before:
                skillStatusData = skillComp.m_currentSkillData.BeforeInfo;
                break;
            case SkillStatusEnum.Current:
                skillStatusData = skillComp.m_currentSkillData.CurrentInfo;
                break;
            case SkillStatusEnum.Later:
                skillStatusData = skillComp.m_currentSkillData.LaterInfo;
                break;
        }

        string FXName = skillStatusData.m_FXName;
        float FXLifeTime = skillStatusData.m_FXLifeTime;
        float offset = skillStatusData.m_FXOffSet;
        HardPointEnum FXCreatPoint = skillStatusData.m_FXCreatPoint;

        string followFXName = skillStatusData.m_FollowFXName;
        float FLoffset = skillStatusData.m_FollowFXOffSet;
        HardPointEnum followFXCreatPoint = skillStatusData.m_FollowFXCreatPoint;
        float followFXLifeTime = skillStatusData.m_FollowFXLifeTime;

        //Debug.Log("FXName " + FXName);
        //Debug.Log("followFXName " + followFXName);

        //现在不管特效填跟不跟随都跟随角色，并在技能被打断时退出
        if (FXName != "null")
        {
            //恢复不follow
            CreateEffect(entity,FXName, FXCreatPoint, FXLifeTime, offset);
        }

        if (followFXName != "null")
        {
            CreateFollowSkillEffect(entity, followFXName, followFXCreatPoint, followFXLifeTime);
        }
    }

    int GetDelayTime(SkillStatusComponent skillComp)
    {
        SkillStatusDataGenerate skillStatusData = null;
        switch (skillComp.m_skillStstus)
        {
            case SkillStatusEnum.Before:
                skillStatusData = skillComp.m_currentSkillData.BeforeInfo;
                break;
            case SkillStatusEnum.Current:
                skillStatusData = skillComp.m_currentSkillData.CurrentInfo;
                break;
            case SkillStatusEnum.Later:
                skillStatusData = skillComp.m_currentSkillData.LaterInfo;
                break;
        }
        float delayTime = skillStatusData.m_FXDelay;

        return (int)(delayTime * 1000);
    }

    public void CreateEffect(EntityBase entity, string effectName, HardPointEnum hardPoint, float time,float offset)
    {
        PerfabComponent pc = entity.GetComp<PerfabComponent>();
        SkillStatusComponent ssc = entity.GetComp<SkillStatusComponent>();
        MoveComponent mc = entity.GetComp<MoveComponent>();

        //挂载点完全放弃了
        Transform creatPoint = pc.hardPoint.hardPoint.GetHardPoint(hardPoint);

        Vector3 createPos = mc.pos.ToVector() + ssc.skillDir.ToVector() * offset;
        Vector3 charactorEugle = Vector3.zero;
        Vector3 m_aimWaistDir = ssc.skillDir.ToVector();
        float euler = Mathf.Atan2(m_aimWaistDir.x, m_aimWaistDir.z) * Mathf.Rad2Deg;

        if (m_aimWaistDir.z == 0)
        {
            euler = 0;
        }

        charactorEugle.y = euler;

        TransfromComponent etc = new TransfromComponent();
        etc.pos.FromVector(createPos);
        etc.dir.FromVector(m_aimWaistDir);

        AssetComponent eac = new AssetComponent();
        eac.m_assetName = effectName;

        LifeSpanComponent elc = new LifeSpanComponent();
        elc.lifeTime = (int)(time * 1000);

        m_world.CreateEntity("Effect"+ effectName + entity.ID + createPos, etc, eac, elc);
    }

    public void CreateFollowSkillEffect(EntityBase entity, string effectName, HardPointEnum hardPoint, float time)
    {
        PerfabComponent pc = entity.GetComp<PerfabComponent>();

        EffectData data = new EffectData();
        PoolObject effectP = GameObjectManager.GetPoolObject(effectName, pc.hardPoint.hardPoint.GetHardPoint(hardPoint).gameObject);

        effectP.transform.localPosition = Vector3.zero;
        effectP.transform.localEulerAngles = Vector3.zero;

        data.m_effect = effectP;
        data.m_timer = time;

        //m_SkillEffectList.Add(data);

        //Debug.Log("CreateSkillEffect " + effectName + " time " + time + " " + " hard " +  m_character.m_hardPoint.GetHardPoint(hardPoint).gameObject, effectP );
    }

    public void CreateSkillAreaTip(EntityBase entity, SkillStatusComponent status)
    {
        if (RangeTip == null)
        {
            GameObject tip = GameObjectManager.CreateGameObjectByPool("AreaTips");
            RangeTip = tip.GetComponent<CreatMesh>();
        }
        RangeTip.gameObject.SetActive(true);
        RangeTip.SetMesh(entity, status.m_currentSkillData.SkillInfo.m_EffectArea, true, status.m_currentSkillData.SkillInfo.m_AreaTexture);
    }

    class EffectData
    {
        public PoolObject m_effect;
        public float m_timer;
    }
}
