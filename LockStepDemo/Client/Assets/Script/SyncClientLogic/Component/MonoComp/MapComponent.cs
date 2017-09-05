using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComponent : MonoBehaviour
{

    public Area GetAreaData()
    {
        Area a = new Area();

        a.position = transform.position;
        a.direction = transform.forward;
        
        if(GetComponent<BoxCollider>() != null)
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            a.areaType = AreaType.Rectangle;
            //z方向是长
            //x方向是宽

            a.length = transform.localScale.z * bc.size.z;
            a.Width = transform.localScale.x * bc.size.x;
        }
        else if (GetComponent<CapsuleCollider>() != null)
        {
            CapsuleCollider cc = GetComponent<CapsuleCollider>();
            a.areaType = AreaType.Circle;
            a.angle = 360;
            a.radius = ((transform.localScale.x + transform.localScale.z) / 2) * cc.radius;
        }
        else
        {
            Debug.LogError(name + " not find shape !");
        }


        return a;
    }
}
