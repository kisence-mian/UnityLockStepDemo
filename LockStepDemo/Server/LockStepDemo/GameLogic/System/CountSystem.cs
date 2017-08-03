using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CountSystem : SystemBase
{
    int count = 0;

    public override void FixedUpdate(int deltaTime)
    {
        count++;

        Debug.Log("Count " + count + " deltaTime :" + deltaTime);
    }

}