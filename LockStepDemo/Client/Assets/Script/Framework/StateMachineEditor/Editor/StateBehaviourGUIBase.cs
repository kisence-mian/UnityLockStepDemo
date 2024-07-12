using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBehaviourGUIBase  {
    public MachineStateGUI stateGUI;
    public object target;
    public virtual void OnInspectorGUI()
    {
        target = EditorDrawGUIUtil.DrawClassData(target);
    }
}
