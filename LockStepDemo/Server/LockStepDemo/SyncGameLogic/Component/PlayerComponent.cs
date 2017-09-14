using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerComponent : MomentComponentBase
{
    public string nickName;
    public string characterID;

    public int score = 0;

    private PlayerDataGenerate characterData;

    public SyncVector3 faceDir = new SyncVector3();

    public List<ElementData> elementData = new List<ElementData>();

    public List<BuffInfo> buffList = new List<BuffInfo>();

    public override MomentComponentBase DeepCopy()
    {
        PlayerComponent pc = new PlayerComponent();

        pc.faceDir = faceDir.DeepCopy();
        pc.elementData.Clear();
        pc.characterID = characterID;
        pc.nickName = nickName;
        pc.score = score;

        for (int i = 0; i < elementData.Count; i++)
        {
            pc.elementData.Add(elementData[i].DeepCopy());
        }

        for (int i = 0; i < buffList.Count; i++)
        {
            pc.buffList.Add(buffList[i].DeepCopy());
        }

        return pc;
    }

    #region 赋值方法
    public void AddElement(int elementID)
    {
        for (int i = 0; i < elementData.Count; i++)
        {
            if (elementData[i].id == elementID)
            {
                elementData[i].num++;
            }
        }
    }

    public BuffInfo AddBuff(string buffID, int creater)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].buffID == buffID
                || buffList[i].creater == creater
                )
            {
                buffList[i].buffCount++;
                buffList[i].buffTime = 0;
                return buffList[i];
            }
        }

        BuffInfo bi = new BuffInfo();
        bi.buffID = buffID;
        bi.creater = creater;

        buffList.Add(bi);

        return bi;
    }

    #endregion

    #region 取值方法

    public PlayerDataGenerate CharacterData
    {
        get
        {
            if (characterData == null)
            {
                characterData = DataGenerateManager<PlayerDataGenerate>.GetData(characterID);
            }
            return characterData;
        }
    }

    public int GetSpeed()
    {
        float speed = CharacterData.m_movespeed;

        if (buffList.Count != 0)
        {
            float changeNumber = 0;
            float changePercantage = 1;

            for (int i = 0; i < buffList.Count; i++)
            {
                changeNumber += buffList[i].BuffData.m_SpeedChange;
                changePercantage *= buffList[i].BuffData.m_SpeedChangePercentage;
            }

            speed *= changePercantage;
            speed += changeNumber;
        }

        return (int)(speed * 1000);
    }

    public bool GetIsDizziness()
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            if(buffList[i].BuffData.m_Dizziness)
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}

public class ElementData
{
    public int id;
    public int num;

    public ElementData DeepCopy()
    {
        ElementData ed = new ElementData();

        ed.id = id;
        ed.num = num;

        return ed;
    }
}

public class BuffInfo
{
    public string buffID;
    public int creater;
    public int buffTime;
    public int buffCount;
    public int hitTime;

    BuffDataGenerate buffData;

    public BuffDataGenerate BuffData
    {
        get
        {
            if (buffData == null)
            {
                buffData = DataGenerateManager<BuffDataGenerate>.GetData(buffID);
            }

            return buffData;
        }
    }

    public BuffInfo DeepCopy()
    {
        BuffInfo bi = new BuffInfo();

        bi.buffID = buffID;
        bi.creater = creater;
        bi.buffTime = buffTime;
        bi.buffCount = buffCount;

        return bi;
    }
}
