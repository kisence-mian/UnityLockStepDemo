using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicObjectBehaviour : StateBaseBehaviour
{
    public LogicObject logicObj = new LogicObject();

# if UNITY_EDITOR
    public MachineState state;

#endif

}
