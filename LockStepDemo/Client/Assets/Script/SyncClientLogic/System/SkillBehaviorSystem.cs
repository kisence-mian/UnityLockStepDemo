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
            typeof(PerfabComponent),
            typeof(PlayerComponent),
        };
    }

    public override void FixedUpdate(int deltaTime)
    {
        List<EntityBase> list = GetEntityList();

        for (int i = 0; i < list.Count; i++)
        {
            SkillStatusLogic(list[i], deltaTime);
        }
    }

    /// <summary>
    /// 这里创建特效和音效
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="deltaTime"></param>
    public void SkillStatusLogic(EntityBase entity, int deltaTime)
    {
        SkillStatusComponent sc = entity.GetComp<SkillStatusComponent>();

        if (sc.m_isHit)
        {
            //TODO 技能触发！
        }

        if((sc.m_skillStstus == SkillStatusEnum.Before
            || sc.m_skillStstus == SkillStatusEnum.Current
            || sc.m_skillStstus == SkillStatusEnum.Later
            )
            && sc.m_isEnter)
        {
            CreatSkillEffect(entity,sc);
        }
    }

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
        HardPointEnum FXCreatPoint = skillStatusData.m_FXCreatPoint;

        string followFXName = skillStatusData.m_FollowFXName;
        HardPointEnum followFXCreatPoint = skillStatusData.m_FollowFXCreatPoint;
        float followFXLifeTime = skillStatusData.m_FollowFXLifeTime;

        //Debug.Log("FXName " + FXName);
        //Debug.Log("followFXName " + followFXName);

        //现在不管特效填跟不跟随都跟随角色，并在技能被打断时退出
        if (FXName != "null")
        {
            //恢复不follow
            CreateEffect(entity,FXName, FXCreatPoint, FXLifeTime);
        }

        if (followFXName != "null")
        {
            CreateFollowSkillEffect(entity, followFXName, followFXCreatPoint, followFXLifeTime);
        }
    }

    public void CreateEffect(EntityBase entity, string effectName, HardPointEnum hardPoint, float time)
    {
        PerfabComponent pc = entity.GetComp<PerfabComponent>();
        PlayerComponent plc = entity.GetComp<PlayerComponent>();

        //Transform creatPoint = pc.hardPoint.hardPoint.GetHardPoint(hardPoint);

        Vector3 charactorEugle = Vector3.zero;
        Vector3 m_aimWaistDir = plc.faceDir.ToVector();
        float euler = Mathf.Atan2(m_aimWaistDir.x, m_aimWaistDir.z) * Mathf.Rad2Deg;

        if (m_aimWaistDir.z == 0)
        {
            euler = 0;
        }

        charactorEugle.y = euler + 20;

        //特效 短暂存在
        if (time != -1)
        {
            //EffectManager.ShowEffect(
            //    effectName,
            //    creatPoint.position,
            //    charactorEugle,
            //    time);
        }
        //特效持续存在
        else
        {
            //PoolObject effectP = GameObjectManager.GetPoolObject(
            //    effectName,
            //    creatPoint.gameObject);
            //GameObject effect = effectP.gameObject;

            //effect.transform.localPosition = Vector3.zero;
            //effect.transform.localEulerAngles = Vector3.zero;

            //pc.followEffect.Add(effectP);
        }
    }

    public void CreateFollowSkillEffect(EntityBase entity, string effectName, HardPointEnum hardPoint, float time)
    {
        PerfabComponent pc = entity.GetComp<PerfabComponent>();

        EffectData data = new EffectData();
        //PoolObject effectP = GameObjectManager.GetPoolObject(effectName, pc.hardPoint.hardPoint.GetHardPoint(hardPoint).gameObject);

        //effectP.transform.localPosition = Vector3.zero;
        //effectP.transform.localEulerAngles = Vector3.zero;

        //data.m_effect = effectP;
        //data.m_timer = time;

        //m_SkillEffectList.Add(data);

        //Debug.Log("CreateSkillEffect " + effectName + " time " + time + " " + " hard " +  m_character.m_hardPoint.GetHardPoint(hardPoint).gameObject, effectP );
    }

    class EffectData
    {
        public PoolObject m_effect;
        public float m_timer;
    }
}
