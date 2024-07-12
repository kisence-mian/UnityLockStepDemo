using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioCompmoent))]
public class FlyObjectBase : PoolObject 
{
    public AudioCompmoent m_audio;

    public int m_FlyObjID;
    public string m_skillID;

    [HideInInspector]
    public FlyDataGenerate m_property;

    public int     m_createrID;
    public Vector3 m_dir;
    public float   m_lifeTime = 5;
    public int     m_flyDamage;
    public bool    m_isAlive = true;
    public Camp    m_camp;

    public List<int> m_hitList = new List<int>();

    void Awake()
    {
        m_audio = GetComponent<AudioCompmoent>();
        if (m_audio != null)
        {
            m_audio.OnCreate();
        }
        else
        {
            Debug.LogError(name + " AudioCompmoent is Null!");
        }
    }

    public void Init(int flyID, string skillID, int createrID,string flyName,Vector3 pos,Vector3 dir)
    {
        SkillDataGenerate skillData = DataGenerateManager<SkillDataGenerate>.GetData(skillID);

        m_isAlive = true;
        m_FlyObjID = flyID;
        m_skillID = skillID;
        m_createrID = createrID;
        m_lifeTime = m_property.m_LiveTime;
        m_flyDamage = skillData.m_FlyDamageValue;

        m_camp = GetCreater().m_camp;

        m_hitList.Clear();

        SetPos(skillData,pos,GetCreater());
        SetDir(dir);
    }

    void SetPos(SkillDataGenerate skillData,Vector3 offset,CharacterBase creater)
    {
        //Vector3 pos = offset;

        //if (skillData.m_FlyCreatPoint != HardPointEnum.enemy)
        //{
        //    pos += creater.m_hardPoint.GetHardPoint(skillData.m_FlyCreatPoint).position;
        //}
        offset.y = 1;

        transform.position = offset;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_isAlive)
        {
            m_lifeTime -= Time.deltaTime;

            //飞行物时间到了
            if (m_lifeTime < 0 + SyncService.SyncAheadTime)
            {
                //FightLogicService.FlyTrigger(this, null);

                //if (!m_property.m_CollisionTrigger)
                //{
                //    FightLogicService.CalcFlyDamage(this,true);
                //}
                return;
            }

            if (m_property != null)
            {
                transform.Translate(m_dir * m_property.m_Speed * Time.smoothDeltaTime, Space.World);

                if (m_property.m_CollisionTrigger)
                {
                    //FightLogicService.CalcFlyDamage(this,false);
                }
            }
            else
            {
                Debug.LogError("flyObject name:->" + this.gameObject.name+ "<- skillID: ->" + m_skillID + "<- flyID:" + m_FlyObjID + " property is null", this.gameObject);
            }
        }
	}

    public void SetDir(Vector3 dir)
    {
        transform.forward = dir;
        m_dir = dir;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (m_isAlive)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("zudang"))
            {
                //FightLogicService.FlyTrigger(this, null);
            }
        }
    }

    public void ShowHitEffect()
    {
        EffectManager.ShowEffect(m_property.m_HitEffect, transform.position + transform.forward * 0.3f, 1);
        if(m_property.m_HitSFX != "null")
        {
            //AudioManager.PlaySound2D(m_property.m_HitSFX);
            m_audio.PlayAudio(m_property.m_HitSFX);
        }
    }

    #region 获取数据方法

    Area DamageArea;
    public Area GetDamageArea()
    {
        if (DamageArea == null)
        {
            DamageArea = new Area();
            DamageArea.areaType = AreaType.Circle;
            DamageArea.radius = m_property.m_Radius;
        }

        DamageArea.direction = transform.forward;
        DamageArea.position = transform.position;

        return DamageArea;
    }

    public CharacterBase GetCreater()
    {
        return CharacterManager.GetCharacter(m_createrID);
    }


    #endregion

}
