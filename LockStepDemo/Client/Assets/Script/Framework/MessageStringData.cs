using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/MessageStringData")]
public class MessageStringData : ScriptableObject
{
    public List<MessageString> mesList = new List<MessageString>();

    public MessageString GetMessageString(string name)
    {
        for (int i = 0; i < mesList.Count; i++)
        {
            if (mesList[i].name == name)
                return mesList[i];
        }
        return null;
    }

    public string GetValue(string name)
    {
        MessageString ms = GetMessageString(name);
        if (ms == null)
            return null;
        else
            return ms.value;
    }

    public void SetValue(string name,string value)
    {
        MessageString ms = GetMessageString(name);
        if (ms == null)
        {
            mesList.Add(new MessageString(name, value));
        }
        else
        {
            ms.value = value;
        }
    }
    public void RemoveItem(string name)
    {
        MessageString ms = GetMessageString(name);
        if (ms != null)
        {
            mesList.Remove(ms);
        }
    }
}

[System.Serializable]
public class MessageString
{
    public string name = "";
    public string value = "";

    public MessageString(string name,string value)
    {
        this.name = name;
        this.value = value;
    }
}
