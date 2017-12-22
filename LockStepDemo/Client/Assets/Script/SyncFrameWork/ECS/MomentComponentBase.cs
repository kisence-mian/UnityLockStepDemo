using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 所有需要网络同步的继承这个组件
/// </summary>
public abstract class MomentComponentBase : ComponentBase
{
    private int id;
    private int frame;
    private bool isChange;

    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public int Frame
    {
        get
        {
            return frame;
        }

        set
        {
            frame = value;
        }
    }

    public bool IsChange
    {
        get
        {
            return isChange;
        }

        set
        {
            isChange = value;
        }
    }

    public abstract MomentComponentBase DeepCopy();
}
