using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayText : PoolObject
{
    Text m_text;

    List<Image> m_animObjectList_Image = new List<Image>();
    List<Text>  m_animObjectList_Text = new List<Text>();
    //List<Color> m_oldColor = new List<Color>();

    public Font m_braveFont;
    public Font m_largeFont;
    public Font m_middleFont;
    public Font m_smallFont;
    public Font m_recoverFont;

    public override void OnCreate()
    {
        m_text = GetComponent<Text>();
        UguiAlphaInit();
    }

    public void SetText(string content)
    {
        m_text.text = content;
    }

    public void ChangeFontSize(int fontSize)
    {
        m_text.fontSize = fontSize;
    }

    //public void ChangeFontColor(Color fontColor, Color outLineColor)
    //{
    //    m_text.color = fontColor;
    //    m_outLine.effectColor = outLineColor;
    //}

    public void ChangeFont(FontType fontType)
    {
        Font myFont = m_braveFont;
        switch (fontType)
        {
            case FontType.Brave : myFont = m_braveFont;break;
            case FontType.Small : myFont = m_smallFont;break;
            case FontType.Middle : myFont = m_middleFont;break;
            case FontType.Large : myFont = m_largeFont;break;
            case FontType.Recover : myFont = m_recoverFont;break;
        }

        m_text.font = myFont;
 
    }

    Vector3 positionOffset = new Vector3(50, 300, 0);
    Vector3 ScaleTmp = new Vector3(3,3,3);
    public void ShowAnim()
    {
        AnimSystem.UguiMove(gameObject, null, transform.position + positionOffset, 0.2f);
        AnimSystem.Scale(gameObject, Vector3.zero, ScaleTmp, 0.2f, InterpType.Default);
        AnimSystem.Scale(gameObject, ScaleTmp, Vector3.one, 0.1f, InterpType.Default, delayTime: 0.2f);

        AnimSystem.CustomMethodToFloat(SetAlpha, 0, 1, 0.2f);
        AnimSystem.CustomMethodToFloat(SetAlpha, 1, 0.3f, 0.5f, 0.3f);

        Timer.DelayCallBack(0.6f, DestroyThis);
    }

    public void DestroyThis(params object[] obj)
    {
        GameObjectManager.DestroyPoolObject(this);
    }

    public void UguiAlphaInit()
    {
        m_animObjectList_Image.Clear();
        m_animObjectList_Text.Clear();
        //m_oldColor.Clear();

        Image[] images = gameObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].transform.GetComponent<Mask>() == null)
            {
                m_animObjectList_Image.Add(images[i]);
               // m_oldColor.Add(images[i].color);
            }
        }
    }

    Color colTmp = new Color(1,1,1,1);
    public void SetAlpha(float a)
    {
        Color newColor = colTmp;

        int index = 0;
        for (int i = 0; i < m_animObjectList_Image.Count; i++)
        {
            //newColor = m_oldColor[index];
            newColor.a = a;
            m_animObjectList_Image[i].color = newColor;

            index++;
        }

        for (int i = 0; i < m_animObjectList_Text.Count; i++)
        {
            //newColor = m_oldColor[index];
            newColor.a = a;
            m_animObjectList_Text[i].color = newColor;

            index++;
        }
    }
}

public enum FontType
{
    Brave,
    Large,
    Middle,
    Small,
    Recover

}
