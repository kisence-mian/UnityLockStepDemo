using UnityEngine;
using System.Collections;

public class WaitMatchWindow : UIWindowBase 
{
    AnimData m_loopAnim = null;
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_Canel", ClickCanel);
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(GetGameObject("Text_Title"), new Vector3(0, 1000, 0), new Vector3(0,150,0),interp: InterpType.OutExpo);
        AnimSystem.UguiMove(GetGameObject("Button_Canel"), new Vector3(0, -500, 0), new Vector3(0, 170, 0), interp: InterpType.OutExpo);

        AnimSystem.UguiAlpha(m_uiRoot, 0, 1, callBack:(object[] obj)=>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        m_loopAnim = AnimSystem.Rotate(GetGameObject("Image_Rotation"), Vector3.zero, new Vector3(0, 0, 360),2f, repeatType: RepeatType.Loop);

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(GetGameObject("Text_Title"), null,new Vector3(0, 1000, 0), interp: InterpType.InExpo);
        AnimSystem.UguiMove(GetGameObject("Button_Canel"),null, new Vector3(0, -500, 0), interp: InterpType.InExpo);

        AnimSystem.UguiAlpha(m_uiRoot, null, 0, callBack:(object[] obj) =>
        {
            StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        });

        AnimSystem.StopAnim(m_loopAnim);
        m_loopAnim = null;

        yield return new WaitForEndOfFrame();
    }

    public void ClickCanel(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<MainMenuStatus>().CenelMatch();
    }
}