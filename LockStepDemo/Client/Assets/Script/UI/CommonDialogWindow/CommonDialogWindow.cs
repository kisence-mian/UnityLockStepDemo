using UnityEngine;
using System.Collections;
using System;

public class CommonDialogWindow : UIWindowBase 
{
    public static void Open(string content, string[] buttonTexts, CallBack[] callBacks = null)
    {
        Debug.Log(" CommonDialogWindow.Open :" + content);
        UIManager.OpenUIWindow("CommonDialogWindow", (win, param) =>
         {
             ((CommonDialogWindow)win).Initialize(content, buttonTexts, callBacks);

         });
    }
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_Center", Button_0);
        AddOnClickListener("Button_Left", Button_0);
        AddOnClickListener("Button_Right", Button_1);
    }


    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {
       
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        //AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
        //{
          StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        //});

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        //AnimSystem.UguiAlpha(gameObject , null, 0, callBack:(object[] obj) =>
        //{
            StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        //});

        yield return new WaitForEndOfFrame();
    }

    private CallBack[] callBacks = null;
    private string content;
    private string[] buttonTexts;
    public void Initialize( string content, string[] buttonTexts, CallBack[] callBacks = null)
    {
       
        this.callBacks = callBacks;
        this.content = content;
        this.buttonTexts = buttonTexts;

        SetText("Text_Content", content);
        if (buttonTexts.Length <= 1)
        {
            if (buttonTexts.Length > 0)
            {
                SetText("Text_Center", buttonTexts[0]);
            }
            else
            {
                SetText("Text_Center", "Close");
            }
            SetActive("OneButton_Obj", true);
            SetActive("TwoButton_Obj", false);
        }
        else
        {
            SetText("Text_Right", buttonTexts[1]);
            SetText("Text_Left", buttonTexts[0]);
            SetActive("OneButton_Obj", false);
            SetActive("TwoButton_Obj", true);
        }
    }


    public void Button_0(InputUIOnClickEvent inputEvent)
    {
        UIManager.CloseUIWindow (this);

        if (callBacks.Length > 0)
        {
            if (callBacks[0] != null)
                callBacks[0]();
        }

    }
    public void Button_1(InputUIOnClickEvent inputEvent)
    {
        UIManager.CloseUIWindow(this);
        if (callBacks.Length > 1)
        {
            if (callBacks[1] != null)
                callBacks[1]();
        }
    }
}