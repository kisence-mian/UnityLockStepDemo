using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour {

    public ItemType itemType;
   
    private UIBase uiBase;
    private void Awake()
    {
       
    }



    private CallBack<ShowItem> buyCallBack;
    public string itemID;
    public string itemName;
    private CallBack<ShowItem> button_InfoCallBack;
    public void Initialize(ItemType itemType,string itemID, string itemName,string itemImageName,string moneyNumber, 
        CallBack<ShowItem> buyCallBack,
         CallBack<ShowItem> button_InfoCallBack
        )
    {
        uiBase = GetComponent<UIBase>();
        uiBase.AddOnClickListener("Button_BG", Button_BG);
        uiBase.AddOnClickListener("Button_Info", Button_Info);
        IsBuy = false;
        IsChoose = false;
        this.itemType = itemType;
        this.itemName = itemName;
        uiBase.SetText("Text_Name", itemName);
        uiBase.SetText("Text_MoneyNumber", moneyNumber);
        if (!string.IsNullOrEmpty(itemImageName))
        {
            Texture2D tex = ResourceManager.Load<Texture2D>(itemImageName);
            uiBase.GetImage("Image_Character").sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        this.itemID = itemID;
        this.buyCallBack = buyCallBack;
        this.button_InfoCallBack = button_InfoCallBack;
    }
    /// <summary>
    /// 非消耗品是否已经购买了
    /// </summary>
    private bool isBuy = false;

    public bool IsBuy
    {
        get
        {
            return isBuy;
        }

        set
        {
            isBuy = value;
            if (itemType == ItemType.Non_Consumable)
            {
                uiBase.SetActive("Text_MoneyNumber", !isBuy);
                uiBase.SetActive("Image_Icon", !isBuy);
            }
            
        }
    }

    private bool isChoose = false;
    public bool IsChoose
    {
        get
        {
            return isChoose;
        }

        set
        {
            isChoose = value;

            uiBase.SetActive("Image_BG_Money", !isChoose);
            uiBase.SetActive("Image_BG_Money_Choose", isChoose);
            uiBase.SetActive("Image_Info_Choose", isChoose);
            uiBase.SetActive("Image_BG_Name_Choose", isChoose);
            uiBase.SetActive("Image_BG_Choose", isChoose);
        }
    }
    public CallBack<ShowItem> chooseCallBack;
  

    private void Button_Info(InputUIOnClickEvent inputEvent)
    {
        if (button_InfoCallBack != null)
            button_InfoCallBack(this);
    }

    private void Button_BG(InputUIOnClickEvent inputEvent)
    {
        if(itemType == ItemType.Consumable)
        {
            if (buyCallBack != null)
                buyCallBack(this);
        }
        else
        {
            if (IsBuy)
            {
                IsChoose = true;
                if (chooseCallBack != null)
                {
                    chooseCallBack(this);
                }
            }
            else
            {
                if (buyCallBack != null)
                    buyCallBack(this);
            }
        }
        
    }
}

public enum ItemType
{
    /// <summary>
    /// 消耗品
    /// </summary>
    Consumable,
    /// <summary>
    /// 非消耗品
    /// </summary>
    Non_Consumable,
}
