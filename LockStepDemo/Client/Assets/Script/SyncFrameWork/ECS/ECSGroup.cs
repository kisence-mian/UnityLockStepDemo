using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECSGroup  {
    private string name;

    public string Name
    {
        get
        {
            return name;
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

    public ECSGroup(string name, string[] components)
    {
        this.name = name;
        this.components = components;
    }
}
