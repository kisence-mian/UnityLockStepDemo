using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectCompmoent : CompmoentBase 
{
    List<PoolObject> m_effectList = new List<PoolObject>();
    List<EffectData> m_SkillEffectList = new List<EffectData>();

    public void CreatSkillEffect(string l_s_attackSkillID, SkillStatusEnum l_attackStatus, bool isFollow = false)
    {
        string status = "null";
        switch (l_attackStatus)
        {
            case SkillStatusEnum.Before:
                status = DataGenerateManager<SkillDataGenerate>.GetData(l_s_attackSkillID).m_BeforeStatus;
                break;
            case SkillStatusEnum.Current:
                status = DataGenerateManager<SkillDataGenerate>.GetData(l_s_attackSkillID).m_CurrentStatus;
                break;
            case SkillStatusEnum.Later:
                status = DataGenerateManager<SkillDataGenerate>.GetData(l_s_attackSkillID).m_LaterStatus;
                break;
        }

        SkillStatusDataGenerate skillStatusData = DataGenerateManager<SkillStatusDataGenerate>.GetData(status);
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
            //CreateSkillEffect(FXName, FXCreatPoint, FXLifeTime);
            //恢复不follow
            CreateEffect(FXName, FXCreatPoint, FXLifeTime);
        }

        if (followFXName != "null")
        {
            CreateSkillEffect(followFXName, followFXCreatPoint, followFXLifeTime);
            //character.m_effectComp.CreateEffectInCharacter(followFXName, followFXCreatPoint, followFXLifeTime);
        }
    }

    public override void OnUpdate()
    {
        for (int i = 0; i < m_SkillEffectList.Count; i++)
        {
            if (m_SkillEffectList[i].m_timer != -1)
            {
                m_SkillEffectList[i].m_timer -= Time.deltaTime;

                if (m_SkillEffectList[i].m_timer < 0)
                {
                    GameObjectManager.DestroyPoolObject(m_SkillEffectList[i].m_effect);
                    m_SkillEffectList.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void CreateSkillEffect(string effectName, HardPointEnum hardPoint, float time = -1)
    {
        EffectData data = new EffectData();
        PoolObject effectP = GameObjectManager.GetPoolObject(effectName,m_character.m_hardPoint.GetHardPoint(hardPoint).gameObject);

        effectP.transform.localPosition = Vector3.zero;
        effectP.transform.localEulerAngles = Vector3.zero;

        data.m_effect = effectP;
        data.m_timer = time;

        m_SkillEffectList.Add(data);

        //Debug.Log("CreateSkillEffect " + effectName + " time " + time + " " + " hard " +  m_character.m_hardPoint.GetHardPoint(hardPoint).gameObject, effectP );
    }

    public void BreakSkillEffect()
    {
        //Debug.Log("BreakSkillEffect");

        //for (int i = 0; i < m_SkillEffectList.Count; i++)
        //{
        //     GameObjectManager.DestroyPoolObject(m_SkillEffectList[i].m_effect);
        //}

        //m_SkillEffectList.Clear();
    }

    //跟随特效
    public void CreateEffectInCharacter(string effectName, HardPointEnum hardPoint, float time = -1)
    {
        Transform creatPoint = m_character.m_hardPoint.GetHardPoint(hardPoint);
        Vector3 charactorEugle = m_character.m_waistNode.transform.localEulerAngles;
        //Debug.Log(creatPoint);
        //特效 短暂存在
        if (time != -1)
        {
            EffectManager.ShowEffect(
                 effectName,
                 creatPoint.gameObject,
                 charactorEugle,
                 time);
        }
        //特效持续存在
        else
        {
            PoolObject effectP = GameObjectManager.GetPoolObject(
                effectName,
                m_character.m_hardPoint.GetHardPoint(hardPoint).gameObject);
           
            GameObject effect = effectP.gameObject;

            effect.transform.localPosition = Vector3.zero;
            effect.transform.localEulerAngles = charactorEugle;
            //effect.transform.localScale = Vector3.one;

            m_effectList.Add(effectP);
        }
    }


    //不跟随的特效
    public void CreateEffect(string effectName, HardPointEnum hardPoint, float time = -1)
    {
        Debug.Log("CreateEffect 不跟随的特");

        Transform creatPoint= m_character.m_hardPoint.GetHardPoint(hardPoint);
        Vector3 charactorEugle = Vector3.zero;

        Vector3 m_aimWaistDir = m_character.m_moveComp.m_aimWaistDir;
        float euler = Mathf.Atan2(m_aimWaistDir.x, m_aimWaistDir.z) * Mathf.Rad2Deg;

        if (m_aimWaistDir.z == 0)
        {
            euler = 0;
        }


        if (m_character.m_waistNode != null)
        {
            charactorEugle.y = euler + 20;
        }

        //特效 短暂存在
        if (time != -1)
        {
            EffectManager.ShowEffect(
                effectName,
                creatPoint.position,
                charactorEugle,
                time);
        }
        //特效持续存在
        else
        {
            PoolObject effectP = GameObjectManager.GetPoolObject(
                effectName,
                creatPoint.gameObject);
            GameObject effect = effectP.gameObject;

            effect.transform.localPosition = Vector3.zero;
            effect.transform.localEulerAngles = Vector3.zero;

            m_effectList.Add(effectP);
        }
 
    }

    public void RemoveEffect(string effectName)
    {
        PoolObject effectP  = GetEffect(effectName);
        if (effectP != null)
        {
            GameObjectManager.DestroyPoolObject(effectP);
            m_effectList.Remove(effectP);
        }
    }

    public override void Destroy()
    {
        BreakSkillEffect();

        for (int i = 0; i < m_effectList.Count; i++)
        {
            GameObjectManager.DestroyPoolObject(m_effectList[i]);
        }

        m_effectList.Clear();
    }

    PoolObject GetEffect(string effectName)
    {
        for (int i = 0; i < m_effectList.Count; i++)
        {
            if (m_effectList[i].name == effectName)
            {
                return m_effectList[i];
            }
        }

        return null;
    }

    class EffectData
    {
        public PoolObject m_effect;
        public float m_timer;
    }
}
