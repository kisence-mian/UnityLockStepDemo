using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorDrawToolBarGUI  {

    /// <summary>
    /// 类VS代码标签UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scrollPos"></param>
    /// <param name="contentList"></param>
    /// <param name="select"></param>
    /// <param name="OnChangeSelectItemCallback">标签选择切换</param>
    /// <param name="OnMovePositionItemCallback">向右移动标签为true，向左为false</param>
    /// <param name="OnCloseItemCallback">标签关闭</param>
    /// <returns></returns>
	public static ToolBarData<T> DrawToolBar<T>(ref Vector2 scrollPos, List<ToolBarData<T>> contentList, ToolBarData<T> select, 
        CallBack<ToolBarData<T>> OnChangeSelectItemCallback = null, 
        CallBack<bool, ToolBarData<T>> OnMovePositionItemCallback = null, 
        CallBack<ToolBarData<T>> OnCloseItemCallback=null)
    {
        int selectID = -1;
        if (select != null)
            selectID = select.Id;
        else
            selectID = -1;
        if (select==null || !contentList.Contains(select))
        {
            if (contentList.Count > 0)
            {
                select = contentList[0];
            }
        }
      
        GUILayout.BeginHorizontal("AppToolbar");    
        ToolBarData<T>[] arr = contentList.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (DrawItem(contentList, arr[i], ref select, OnMovePositionItemCallback, OnCloseItemCallback))
                break;
        }
        if (selectID != -1 )
        {
            if(select!=null && select.Id != selectID )
            {
                if (OnChangeSelectItemCallback != null)
                    OnChangeSelectItemCallback(select);
            }
        }
        else
        {
            if (select != null)
            {
                if (OnChangeSelectItemCallback != null)
                    OnChangeSelectItemCallback(select);
            }
        }
        GUILayout.FlexibleSpace();    
        GUILayout.EndHorizontal();
        return select;
    }

    private static bool DrawItem<T>(List<ToolBarData<T>> contentList, ToolBarData<T> content,ref ToolBarData<T> contentSelect, CallBack<bool, ToolBarData<T>> OnMovePositionItemCallback = null,  CallBack<ToolBarData<T>> closeItemCallback = null)
    {
        bool result = false;
        bool isSelect = content.Id.Equals(contentSelect.Id);
        int id = contentList.IndexOf(content);
        GUIStyle style = "Label";
        GUIStyle style1 = "Label";
        if (isSelect)
            style1 = "ProgressBarBar";
        GUILayout.BeginHorizontal(style1,GUILayout.MinWidth(100),GUILayout.MaxWidth(160));
        if(isSelect && (id>0)&& GUILayout.Button("<", style, GUILayout.Width(10)))
        {
            contentList.Remove(content);
            contentList.Insert(id - 1, content);
            if (OnMovePositionItemCallback != null)
            {
                OnMovePositionItemCallback(false, contentSelect);
            }
            result = true;
        }
        if (GUILayout.Button(content.name, style, GUILayout.MaxWidth(60),GUILayout.MaxWidth(120)))
        {
            if (!isSelect)
                contentSelect = content;
            result = false;
        }
        if (isSelect && GUILayout.Button("X", style, GUILayout.Width(15)))
        {
            contentList.Remove(content);
            result = true;
            if (closeItemCallback != null)
                closeItemCallback(content);
        }
        if (isSelect &&(id< contentList.Count-1) && GUILayout.Button(">", style, GUILayout.Width(15)))
        {
            contentList.Remove(content);
            contentList.Insert(id + 1, content);
            if (OnMovePositionItemCallback != null)
            {
                OnMovePositionItemCallback(true, contentSelect);
            }
            result = true;
        }
       // GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(4);
        return result;
    }
 }
public class ToolBarData<T>
{
    private int id = 0;
    public string name = "";
    public T data;

    private static int count = 0;

    public int Id { get { return id; }  }

    public ToolBarData(string name,T data)
    {
        this.name = name;
        this.data = data;
        count++;
        id = count;
    }
}