using Protocol.fightModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public static List<Item> s_ItemList = new List<Item>();
    public static GameObject s_parent;
    //static int s_ItemIndex = 0;

    public static void Init()
    {
        ApplicationManager.s_OnApplicationUpdate += Update;
        //CharacterManager.AddListener(CharacterEventType.Die.ToString(), RecevideCharacterDie);

        GlobalEvent.AddTypeEvent<createitemcmd_c>(CreateItem);

        s_parent = new GameObject("ItemManager");
    }

    public static void Dispose()
    {
        //s_ItemIndex = 0;
        ApplicationManager.s_OnApplicationUpdate -= Update;

        for (int i = 0; i < s_ItemList.Count; i++)
        {
            GameObjectManager.DestroyPoolObject(s_ItemList[i]);
        }
        s_ItemList.Clear();

        GlobalEvent.RemoveTypeEvent<createitemcmd_c>(CreateItem);

        UnityEngine.Object.Destroy(s_parent);

        //CharacterManager.RemoveListener(CharacterEventType.Die.ToString(), RecevideCharacterDie);
    }

    static void Update()
    {
        for (int i = 0; i < s_ItemList.Count; i++)
        {
            //if (s_ItemList[i].m_isTrigger == false && s_ItemList[i].m_isCanTrigger)
            {
                //List<CharacterBase> list = FightLogicService.GetAreaListSameCamp(s_ItemList[i].m_triggerArea, s_ItemList[i].m_camp);
                //if (list.Count > 0)
                //{
                //    FightLogicService.DealItem(s_ItemList[i]);
                //    s_ItemList[i].m_isTrigger = true;
                //}
            }
        }
    }

    public static void CreateItem(createitemcmd_c cmd,params object[] objs)
    {
        for (int i = 0; i < cmd.list.Count; i++)
        {
            try
            {
                ItemsDataGenerate data = DataGenerateManager<ItemsDataGenerate>.GetData(cmd.list[i].m_itemname);

                Item bv = (Item)GameObjectManager.GetPoolObject(data.m_modelid, s_parent);

                Vector3 pos = new Vector3();

                pos.x = cmd.list[i].m_posx;
                pos.y = cmd.list[i].m_posy;
                pos.z = cmd.list[i].m_posz;

                bv.transform.position = pos;
                bv.m_itemId = cmd.list[i].m_itemid;

                s_ItemList.Add(bv);
            }
            catch (Exception e)
            {
                Debug.LogError("CreateItem error : m_itemid: " + cmd.list[i].m_itemid + " " + cmd.list[i].m_itemname + " exception " + e.ToString());
            }


        }
    }

    public static void PickUpItem(PickUpItemCmd cmd, params object[] objs)
    {
        if (GetItemIsExit(cmd.m_ItemID)
            && CharacterManager.GetCharacterIsExit(cmd.m_characterID))
        {
            Item bv = GetItem(cmd.m_ItemID);
            //Item bvTmp = (Item)GameObjectManager.GetPoolObject(bv.m_effectID);

            if (bv.gameObject.activeSelf)
            {
                bv.gameObject.SetActive(false);
            }

            //bvTmp.transform.position = bv.transform.position;
            //bvTmp.PlayItemAnim(CharacterManager.GetCharacter(cmd.m_characterID));
        }
        else
        {
            Debug.LogError("PickUpItem Error Not Find Item or Character m_ItemID:->" + cmd.m_ItemID + " CharacterID:->" + cmd.m_characterID);
        }
    }

    public static void DestroyItem(DestroyItemCmd cmd, params object[] objs)
    {
        if (GetItemIsExit(cmd.m_ItemID))
        {
            Item bv = GetItem(cmd.m_ItemID);

            s_ItemList.Remove(bv);
            GameObjectManager.DestroyPoolObject(bv);
        }
        else
        {
            Debug.LogError("DestroyItem Error Not Find Item  " + cmd.m_ItemID);
        }
    }

    public static Item GetItem(int ItemID)
    {
        for (int i = 0; i < s_ItemList.Count; i++)
        {
            if (s_ItemList[i].m_itemId == ItemID)
            {
                return s_ItemList[i];
            }
        }

        throw new Exception("Not Find Item by :" + ItemID);
    }

    public static bool GetItemIsExit(int ItemID)
    {
        for (int i = 0; i < s_ItemList.Count; i++)
        {
            if (s_ItemList[i].m_itemId == ItemID)
            {
                return true;
            }
        }

        return false;
    }

    public static void ReceviceItemCmd(ItemCmd bcmd)
    {
        //if (bcmd is CreateItemCmd)
        //{
        //    CreateItem((CreateItemCmd)bcmd);
        //}
        //else 
        if (bcmd is PickUpItemCmd)
        {
            PickUpItem((PickUpItemCmd)bcmd);
        }
        else if (bcmd is DestroyItemCmd)
        {
            DestroyItem((DestroyItemCmd)bcmd);
        }
    }
}
