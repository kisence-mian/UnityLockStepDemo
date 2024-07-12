using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCharacterWindow : UIWindowBase 
{
    const string c_DataName = "PlayerData";
    const string c_ItemName = "SelectCharacterItem";

    DataTable data;

    List<UIBase> m_itemList = new List<UIBase>();

    int m_choiceIndex = 0;

    public DataTable Data
    {
        get
        {
            if(data == null)
            {
                data = DataManager.GetData(c_DataName);
            }

            return data;
        }

        set
        {
            data = value;
        }
    }

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        m_choiceIndex = GetCharacterIndex(UserData.CharacterID);

        AddOnClickListener("Button_Close", OnClickClose);

        for (int i = 0; i < Data.TableIDs.Count; i++)
        {
            PlayerDataGenerate sdata = DataGenerateManager<PlayerDataGenerate>.GetData(Data.TableIDs[i]);

            UIBase item = CreateItem(c_ItemName, "Layout");
            UpdateItem(item, sdata);
            m_itemList.Add(item);
            item.AddOnClickListener("Button_Choice", OnClickSelectPlayer, Data.TableIDs[i]);
        }

        UpdateChoice();
    }

    public override void OnClose()
    {
        CleanItem();
        m_itemList.Clear();
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {
        
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(m_uiRoot, new Vector3(2000, 0, 0), Vector3.zero, interp: InterpType.OutExpo);

        AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(m_uiRoot, null,new Vector3(2000, 0, 0),  interp: InterpType.InExpo);

        AnimSystem.UguiAlpha(gameObject , null, 0, callBack:(object[] obj) =>
        {
            StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    void OnClickClose(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<MainMenuStatus>().SwitchSelectWindow(false);
    }

    void OnClickSelectPlayer(InputUIOnClickEvent e)
    {
        UserData.CharacterID = e.m_pram;
        m_choiceIndex = GetCharacterIndex(e.m_pram);

        UpdateChoice();
    }

    void UpdateChoice()
    {
        for (int i = 0; i < m_itemList.Count; i++)
        {
            SetChoice(m_itemList[i], i == m_choiceIndex);
        }
    }

    void SetChoice(UIBase ui,bool isChoice)
    {
        if(!isChoice)
        {
            ui.SetActive("Button_Choice", true);
            ui.SetActive("Button_Choiced", false);
        }
        else
        {
            ui.SetActive("Button_Choiced", true);
            ui.SetActive("Button_Choice", false);
        }
    }

    void UpdateItem(UIBase ui, PlayerDataGenerate data)
    {
        ui.SetText("Text_characterName", data.m_ModelID);
    }

    int GetCharacterIndex(string characterID)
    {
        for (int i = 0; i < Data.TableIDs.Count; i++)
        {
            PlayerDataGenerate sdata = DataGenerateManager<PlayerDataGenerate>.GetData(Data.TableIDs[i]);

            if(characterID == sdata.m_key)
            {
                return i;
            }
        }

        throw new System.Exception("GetCharacterIndex Exception not find ->" + characterID);
    }
}