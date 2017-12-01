using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class RecordSystemBase :SystemBase
{
    public abstract void Record(int frame);

    public abstract void RevertToFrame(int frame);

    public abstract void ClearAfter(int frame);

    public abstract void ClearBefore(int frame);

    public abstract void ClearAll();

    public virtual MomentComponentBase GetRecord(int id, int frame) { throw new NotImplementedException(); }

    public virtual MomentSingletonComponent GetSingletonRecord(int frame) { throw new NotImplementedException(); }

    public abstract void PrintRecord(int id);
}
