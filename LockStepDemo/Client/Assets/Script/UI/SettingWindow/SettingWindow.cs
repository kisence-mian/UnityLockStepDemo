using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SettingWindow : UIWindowBase 
{
    private Slider slider_Music;
    private Slider slider_Sound;
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        if (slider_Music == null)
        {
            slider_Music = GetSlider("Slider_Music");
            slider_Sound = GetSlider("Slider_Sound");
            slider_Music.onValueChanged.AddListener(OnMusicValueChanged);
            slider_Sound.onValueChanged.AddListener(OnSoundValueChanged);
            slider_Music.value = AudioManager.s_MusicVolume;
            slider_Sound.value = AudioManager.s_SoundVolume;
        }

        AddOnClickListener("Button_MusicAdd", Button_MusicAdd);
        AddOnClickListener("Button_MusicMinus", Button_MusicMinus);
        AddOnClickListener("Button_SoundAdd", Button_SoundAdd);
        AddOnClickListener("Button_SoundMinus", Button_SoundMinus);
        AddOnClickListener("Button_Back", Button_Back);

        
    }

    private void OnSoundValueChanged(float arg0)
    {
        AudioManager.s_SoundVolume = arg0;
        Debug.Log("OnSoundValueChanged");
    }

    private void OnMusicValueChanged(float arg0)
    {
        AudioManager.s_MusicVolume = arg0;
        Debug.Log("OnMusicValueChanged");
    }

    private void Button_Back(InputUIOnClickEvent inputEvent)
    {
        UIManager.CloseUIWindow(this);
    }

    private void Button_SoundMinus(InputUIOnClickEvent inputEvent)
    {
        slider_Sound.value -= 0.1f;
    }

    private void Button_SoundAdd(InputUIOnClickEvent inputEvent)
    {
        slider_Sound.value += 0.1f;
    }

    private void Button_MusicMinus(InputUIOnClickEvent inputEvent)
    {
        slider_Music.value -= 0.1f;
    }

    private void Button_MusicAdd(InputUIOnClickEvent inputEvent)
    {
        slider_Music.value += 0.1f;
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {
      
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiAlpha(gameObject , null, 0, callBack:(object[] obj) =>
        {
            StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }
}