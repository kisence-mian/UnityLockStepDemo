using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.GameLogic
{
    class CountSystem:SystemBase
    {
        int count = 0;

        public override void FixedUpdate(int deltaTime)
        {
            count++;

            Debug.Log("Count " + count + " deltaTime :" + deltaTime);
        }

    }
}
