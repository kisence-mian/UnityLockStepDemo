using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopWindow : UIWindowBase 
{
    private Toggle toggle_hero;
    private Toggle toggle_shop;
    private GameObject content_obj;
    //  private GameObject item_Obj;

    private List<ShopDataGenerate> heroShopDatas;
    private List<ShopDataGenerate> itemShopDatas;
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        if (heroShopDatas == null)
        {
            List<ShopDataGenerate> temp = new List<ShopDataGenerate>(DataGenerateManager<ShopDataGenerate>.GetAllData().Values);
            heroShopDatas = new List<ShopDataGenerate>();
            itemShopDatas = new List<ShopDataGenerate>();

            for (int i = 0; i < temp.Count; i++)
            {
                ShopDataGenerate sd = temp[i];
                if (sd.m_page == (int)ShopItemType.Hero)
                {
                    heroShopDatas.Add(sd);
                }
                else if (sd.m_page == (int)ShopItemType.Item)
                {
                    itemShopDatas.Add(sd);
                }
            }
        }
        AddOnClickListener("Button_Back", Button_Back);
        if (!toggle_hero)
        {
            toggle_hero = GetToggle("Toggle_hero");
            toggle_hero.onValueChanged.AddListener(Toggle_hero_ValueChange);

            toggle_shop = GetToggle("Toggle_shop");
            toggle_shop.onValueChanged.AddListener(Toggle_shop_ValueChange);

            content_obj = GetGameObject("Content");
         //   item_Obj = GetGameObject("Item_Obj");

            Toggle_hero_ValueChange(true);
        }
       
    }

  

    private void Button_Back(InputUIOnClickEvent inputEvent)
    {
        UIManager.CloseUIWindow(this);
    }
    private bool toggle_heroState = false;
    private void Toggle_hero_ValueChange(bool isOn)
    {
        if (toggle_heroState == isOn || isOn == false)
            return;
        CreateAllItem(heroShopDatas);
        Debug.Log("Toggle_hero_ValueChange: "+ isOn);
     

    }
    private bool toggle_shopState = false;
    private void Toggle_shop_ValueChange(bool isOn)
    {
        if (toggle_shopState == isOn || isOn == false)
            return;
        CreateAllItem(itemShopDatas);
        Debug.Log("Toggle_shop_ValueChange: " + isOn);
    }
    private List<ShowItem> allItems = new List<ShowItem>();
    private void CreateAllItem(List<ShopDataGenerate> itemDatas)
    {
        int itemCount = allItems.Count;
        for (int i = 0; i < itemCount; i++)
        {
            allItems[i].gameObject.SetActive(false);
        }

        
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ShopDataGenerate sd = itemDatas[i];
            GameObject obj = null;
            if (i + 1 > itemCount)
            {
                obj = CreateItem("Shop_Item_Obj", "Content").gameObject;
            }
            else
            {
                obj = allItems[i].gameObject;
            }
            obj.SetActive(true);
            ShowItem si = obj.GetComponent<ShowItem>();
            if (!allItems.Contains(si))
            {
                allItems.Add(si);
            }
            ItemType it = (ItemType)sd.m_ItemType;
            si.Initialize(it, sd.m_item_id.ToString(), sd.m_name, sd.m_IconName,sd.m_cost.ToString(), HeroBuyCallBack, HeroButton_InfoCallBack);
            if(it == ItemType.Non_Consumable)
            {
                si.chooseCallBack = HeroChooseCallBack;
            }
        }

        currentChooseItem = null;
    }
    private ShowItem currentChooseItem;
    private void HeroChooseCallBack(ShowItem t)
    {
        if (currentChooseItem)
        {
            currentChooseItem.IsChoose = false;
           
        }
        currentChooseItem = t;
        UserData.CharacterID = currentChooseItem.itemID;
    }

    private void HeroButton_InfoCallBack(ShowItem t)
    {
        
    }

    private void HeroBuyCallBack(ShowItem t)
    {
        CommonDialogWindow.Open("你要购买\"" + t.itemName + "\"", new string[] { "取消", "购买" }, new CallBack[] {
           () =>
           {
              
           },
           () =>
           {
               t.IsBuy = true;
           }

       });
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
public enum ShopItemType
{
    Hero=3,
    Item=4,
}