using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class AudioCompmoent : CompmoentBase
{
    public AudioSource m_audio;

    List<AudioSource> s_2Dplayers = new List<AudioSource>();

    //string m_currentAudioName = "";
    public override void OnCreate()
    {
        base.OnCreate();
        m_audio = GetComponent<AudioSource>();
        AudioManager.s_OnSoundVolumeChange += ReceviceOnSoundChange;
    }

    public override void Destroy()
    {
        AudioManager.s_OnSoundVolumeChange -= ReceviceOnSoundChange;
    }

    public void PlayAudio(string audioName)
    {
        AudioSource tmp = GetAudioSource2D();

        tmp.clip = AudioManager.GetAudioClip(audioName);
        tmp.loop = false;
        tmp.Play();
        tmp.volume = AudioManager.s_SoundVolume;
    }

    public void PlayDelayed(string audioName,float delayTime)
    {
        AudioSource tmp = GetAudioSource2D();
        tmp.clip = AudioManager.GetAudioClip(audioName);
        tmp.loop = false;
        tmp.PlayDelayed(delayTime);
        tmp.volume = AudioManager.s_SoundVolume;
    }

    public AudioSource GetAudioSource2D()
    {
        AudioSource AudioSourceTmp = null;
        for (int i = 0; i < s_2Dplayers.Count; i++)
        {
            AudioSourceTmp = s_2Dplayers[i];
            if (AudioSourceTmp.isPlaying == false)
            {
                return AudioSourceTmp;
            }
        }

        AudioSourceTmp = gameObject.AddComponent<AudioSource>();

        s_2Dplayers.Add(AudioSourceTmp);

        return AudioSourceTmp;
    }

    public void PlaySkillSFX(string attackSkillID, SkillStatusEnum attackStatus)
    {
        string status = "null";
        switch (attackStatus)
        {
            case SkillStatusEnum.Before:
                status = DataGenerateManager<SkillDataGenerate>.GetData(attackSkillID).m_BeforeStatus;
                break;
            case SkillStatusEnum.Current:
                status = DataGenerateManager<SkillDataGenerate>.GetData(attackSkillID).m_CurrentStatus;
                break;
            case SkillStatusEnum.Later:
                status = DataGenerateManager<SkillDataGenerate>.GetData(attackSkillID).m_LaterStatus;
                break;
        }

        string SFXName = DataGenerateManager<SkillStatusDataGenerate>.GetData(status).m_SFXName;
        float delayTime = DataGenerateManager<SkillStatusDataGenerate>.GetData(status).m_SFXDelay;
        if (SFXName != "null")
        {
            //PlayAudio(SFXName);
            PlayDelayed(SFXName, delayTime);
        }
    }

    void ReceviceOnSoundChange(SoundType soundType)
    {
        for (int i = 0; i < s_2Dplayers.Count; i++)
        {
            s_2Dplayers[i].volume = AudioManager.s_SoundVolume;
        }
    }
}
