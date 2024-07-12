using Protocol;
using Protocol.fightModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    static List<p_rank> s_rankLists = new List<p_rank>();
    static List<ElementData> s_elementList = new List<ElementData>(); //所有可用的元素
    static List<p_item_info> s_itemList = new List<p_item_info>();   //所有物品

    static List<int> s_choiceList = new List<int>(); //当前选中的元素列表

    public static List<p_rank> RankLists
    {
        get
        {
            return s_rankLists;
        }

        set
        {
            s_rankLists = value;
            GlobalEvent.DispatchEvent(GameDataEvent.RankList);
        }
    }

    public static List<ElementData> ElementList
    {
        get
        {
            return s_elementList;
        }

        set
        {
            s_elementList = value;
            GlobalEvent.DispatchEvent(GameDataEvent.ElementList);
        }
    }

    public static List<int> ChoiceList
    {
        get
        {
            return s_choiceList;
        }

        set
        {
            s_choiceList = value;
            SendElementList();
            GlobalEvent.DispatchEvent(GameDataEvent.ChoiceList);
        }
    }

    public static List<p_item_info> ItemList
    {
        get
        {
            return s_itemList;
        }

        set
        {
            s_itemList = value;
            GlobalEvent.DispatchEvent(GameDataEvent.ItemList);
        }
    }

    #region 外部调用

    public static void Init()
    {
        Debug.Log("Game Data Init");

        GlobalEvent.AddTypeEvent<fight_rank_c>(ReceviceRankMsg);
        GlobalEvent.AddTypeEvent<fight_element_c>(ReceviceElementMsg);

        GlobalEvent.AddTypeEvent<fight_item_list_c>(ReceviceItemMsg);
        GlobalEvent.AddTypeEvent<fight_item_num_c>(ReceviceChangeItemMsg);
    }

    public static void ClearData()
    {
        s_rankLists = new List<p_rank>();
        //s_elementList = new List<p_item_info>();

        s_itemList = new List<p_item_info>();

        s_choiceList = new List<int>();
    }

    public static void Dispose()
    {
        GlobalEvent.RemoveTypeEvent<fight_rank_c>(ReceviceRankMsg);
        GlobalEvent.RemoveTypeEvent<fight_element_c>(ReceviceElementMsg);

        GlobalEvent.RemoveTypeEvent<fight_item_list_c>(ReceviceItemMsg);
        GlobalEvent.RemoveTypeEvent<fight_item_num_c>(ReceviceChangeItemMsg);
    }

    #endregion

    #region 事件接收

    static void ReceviceRankMsg(fight_rank_c e, params object[] obj)
    {
        RankLists = e.lists;
    }

    static void ReceviceElementMsg(fight_element_c e, params object[] obj)
    {
        //Debug.Log("ReceviceItemMsg " + e.list.Count);

        //ElementList = e.list;
    }

    static void ReceviceItemMsg(fight_item_list_c e, params object[] obj)
    {
        ItemList = e.lists;
    }

    static void ReceviceChangeItemMsg(fight_item_num_c e, params object[] obj)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if(ItemList[i].id == e.id)
            {
                ItemList[i].num = e.num;
                return;
            }
        }

        p_item_info info = new p_item_info();
        info.id = e.id;
        info.num = e.num;

        ItemList.Add(info);
    }

    #endregion

    #region 消息发送

    public static void SendElementList()
    {
        fight_setelement_s msg = new fight_setelement_s();

        msg.item1 = 0;
        msg.item2 = 0;

        if (ChoiceList.Count ==0)
        {

        }
        else if(ChoiceList.Count == 1)
        {
            msg.item1 = ChoiceList[0];
        }
        else
        {
            msg.item1 = ChoiceList[0];
            msg.item2 = ChoiceList[1];
        }

        //ProtocolAnalysisService.SendCommand(msg);
    }

    #endregion
}
public enum GameDataEvent
{
    RankList,
    ElementList,
    ItemList,
    ChoiceList,
}