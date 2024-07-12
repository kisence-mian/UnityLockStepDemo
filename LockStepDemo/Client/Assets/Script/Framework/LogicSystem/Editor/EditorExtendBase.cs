using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorExtendBase  {

    public virtual void EditorOverrideClassGUI(object value) 
    {
         EditorDrawGUIUtil.DrawClassData(value);
    }
    public virtual void EditorExtendGUI(object value) {  }

    public virtual void Close() { }
  
}
