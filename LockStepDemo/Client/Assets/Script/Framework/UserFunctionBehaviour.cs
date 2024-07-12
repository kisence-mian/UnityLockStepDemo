using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFunctionBehaviour : MonoBehaviour {

    public CallBack<GameObject> OnDrawGizmosFun;
    // 绘制路径  
    // 系统函数，自动调用  
    void OnDrawGizmos()
    {
        if (OnDrawGizmosFun != null)
        {
            OnDrawGizmosFun(gameObject);
        }
    }
}
