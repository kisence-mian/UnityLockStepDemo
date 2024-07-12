using UnityEngine;
using System.Collections;
[System.Serializable]
public class BuffBase 
{
    public string m_buffID; //纯粹就是为了在监视面板上看看到
    public string m_skillID;
    public int m_count = 0;
    public bool isFinsih = false;
    public BuffDataGenerate m_buffData;
    CharacterBase m_character;

    public int m_createrID = 0;

    float m_time = 0;
    float m_timeSpace = 0;

    public void Init(CharacterBase character,int createrID,string buffID,string skillID)
    {
        m_buffID = buffID;
        m_character = character;
        m_buffData = DataGenerateManager<BuffDataGenerate>.GetData(buffID);
        m_createrID = createrID;
        m_skillID = skillID;

        if (m_buffData.m_EffectArea != "null")
        {
            m_effectArea = new Area();
            m_effectArea.UpdateArea(m_buffData.m_EffectArea, character);
        }
        else
        {
            m_effectArea = null;
        }

        m_count = 0;
        m_time = m_buffData.m_BuffTime;
        isFinsih = false;
    }

    // 叠加
    public void ResetBuff()
    {
        m_count++;

        if (m_count > m_buffData.m_limit)
        {
            m_count = m_buffData.m_limit;
        }

        m_time = m_buffData.m_BuffTime;
        isFinsih = false;

        if (m_buffData.m_BuffCreateFX != "null")
        {
            m_character.m_effectComp.CreateEffectInCharacter(m_buffData.m_BuffCreateFX, HardPointEnum.position, 0.8f);
        }

        if (m_buffData.m_BuffSFX != "null")
        {
            m_character.m_audio.PlayAudio(m_buffData.m_BuffSFX);
            //AudioManager.PlaySound2D(m_buffData.m_BuffSFX);
        }
    }

	public void Update () 
    {
        if (!isFinsih)
        {
            m_time -= Time.deltaTime;

            if (m_time < 0)
            {
                isFinsih = true;
            }
            else
            {
                OnUpdate();
            }
        }
	}

    public void StartBuff()
    {
        if(m_buffData.m_BuffCreateFX != "null")
        {
            m_character.m_effectComp.CreateEffectInCharacter(m_buffData.m_BuffCreateFX, m_buffData.m_BuffFollowPos, 2f);
        }

        if (m_buffData.m_BuffFX != "null")
        {
            m_character.m_effectComp.CreateEffectInCharacter(m_buffData.m_BuffFX, m_buffData.m_BuffFollowPos);
        }

        if (m_buffData.m_BuffCreateSFX != "null")
        {
            //AudioManager.PlaySound2D(m_buffData.m_BuffCreateSFX);
            m_character.m_audio.PlayAudio(m_buffData.m_BuffCreateSFX);
        }

        ResetProperty();
    }

    public void EndBuff()
    {
        if (m_buffData.m_BuffExitFX != "null")
        {
            m_character.m_effectComp.CreateEffectInCharacter(m_buffData.m_BuffExitFX, m_buffData.m_BuffFollowPos, 2f);
        }

        if (m_buffData.m_BuffFX != "null")
        {
            m_character.m_effectComp.RemoveEffect(m_buffData.m_BuffFX);
        }

        if (m_buffData.m_BuffExitSFX != "null")
        {
            //AudioManager.PlaySound2D(m_buffData.m_BuffExitSFX);
            m_character.m_audio.PlayAudio(m_buffData.m_BuffExitSFX);
        }

        ResetProperty();
    }

    void OnUpdate()
    {
        m_timeSpace -= Time.deltaTime;

        if(m_timeSpace <0)
        {
            if(m_buffData != null)
            {

                m_timeSpace = m_buffData.m_BuffEffectSpace;
            }
            else
            {
                Debug.LogError("Buff 数据丢失！");
            }

            //FightLogicService.DisposeBuffEffect(m_character, this);
        }
    }

    void ResetProperty()
    {
        if (m_buffData.m_Cloaking)
        {
            m_character.ResetVisible();
        }

        if (m_buffData.m_IsTakeOver)
        {
            m_character.ResetTakeOver();
        }

        if (m_buffData.m_TrueSight)
        {
            m_character.ResetTrueSight();
        }
    }

    Area m_effectArea;
    public Area GetEffectArea()
    {
        return m_effectArea;
    }

    public CharacterBase GetCreater()
    {
        if (CharacterManager.GetCharacterIsExit(m_createrID))
        {
            return CharacterManager.GetCharacter(m_createrID);
        }
        else
        {
            //Debug.LogError("BuffBase GetCreater is null !");
            return null;
        }
    }
}
