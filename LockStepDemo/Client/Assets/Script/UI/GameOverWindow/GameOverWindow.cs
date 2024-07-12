using UnityEngine;
using System.Collections;
using System;

public class GameOverWindow : UIWindowBase 
{

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_Return", Button_Return);
        AddOnClickListener("Button_Shop", Button_Shop);
        AddOnClickListener("Button_Continue", Button_Continue);

        //当获得第一名
        if (true)
        {
            SetActive("Image_Center0", true);
            SetActive("Image_Center1", false);
        }
        else
        {
            SetActive("Image_Center0", false);
            SetActive("Image_Center1", true);
        }

        SetText("Text_Score", "1000");
        SetText("Text_Coin", "1000");
        SetText("Text_Highest", "1000");
    }

    private void Button_Continue(InputUIOnClickEvent inputEvent)
    {
       
    }

    private void Button_Shop(InputUIOnClickEvent inputEvent)
    {
       
    }

    private void Button_Return(InputUIOnClickEvent inputEvent)
    {
       
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