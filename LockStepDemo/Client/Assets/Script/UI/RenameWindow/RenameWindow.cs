using UnityEngine;
using System.Collections;
using System;

public class RenameWindow : UIWindowBase 
{

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_Confirm", Button_Confirm);
        AddOnClickListener("Button_Cancel", Button_Cancel);
    }

    private void Button_Cancel(InputUIOnClickEvent inputEvent)
    {
        UIManager.CloseUIWindow(this);
    }

    private void Button_Confirm(InputUIOnClickEvent inputEvent)
    {
        string inputName = GetInputField("InputField_NewName").text;
        if (string.IsNullOrEmpty(inputName) || inputName==UserData.NickName)
        {
            Debug.LogError("名字不能为空或与原来的名字相同！");
            return;
        }

        ApplicationStatusManager.GetStatus<MainMenuStatus>().Rename(inputName);
        UIManager.CloseUIWindow(this);
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