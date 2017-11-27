using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECSGroup  {
    private int key;

    public int Key
    {
        get
        {
            return key;
        }
    }

    public string[] Components
    {
        get
        {
            return components;
        }
    }

    private string[] components;

    public ECSGroup(int key, string[] components)
    {
        this.key = key;
        this.components = components;
    }
}
