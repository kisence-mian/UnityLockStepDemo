using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfabComponent : ComponentBase
{
    public GameObject perfab;
    public HardPointComponent hardPoint;
    public List<PoolObject> followEffect = new List<PoolObject>(); //跟随特效容器
}
