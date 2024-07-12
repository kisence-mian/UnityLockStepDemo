using UnityEngine;
using System.Collections;

public class SelectServiceWindow : UIWindowBase 
{
    const string c_itemName = "SelectServiceWindow_SelectItem";
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_test", OnClickSelectTest);
        AddOnClickListener("Button_test", OnClickSelectTest);
        AddOnClickListener("Button_test", OnClickSelectTest);

        RemoveAllListener();

        AddOnClickListener("Button_test", OnClickSelectTest);
        AddOnClickListener("Button_test", OnClickSelectTest);

        DataTable data = DataManager.GetData("ServerData");

        for (int i = 0; i < data.TableIDs.Count; i++)
        {
            ServerDataGenerate sdata = DataGenerateManager<ServerDataGenerate>.GetData(data.TableIDs[i]);

            UIBase item =  CreateItem(c_itemName, "Layout");
            item.SetText("Text",sdata.m_name);

            item.AddOnClickListener(c_itemName, OnClickSelectServer, data.TableIDs[i]);
        }
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

    public void OnClickSelectServer(InputUIOnClickEvent e)
    {
        ServerDataGenerate sdata = DataGenerateManager<ServerDataGenerate>.GetData(e.m_pram);

        NetworkManager.Init<ProtocolService>(System.Net.Sockets.ProtocolType.Udp);
        NetworkManager.SetServer(sdata.m_Address, sdata.m_port);
        NetworkManager.Connect();
    }

    public void OnClickSelectTest(InputUIOnClickEvent e)
    {
        Debug.Log("Test");
    }
}