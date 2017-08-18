using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//TODO 暂时搁置
public class EntityRecordSystem :RecordSystemBase
{
    public override void Init()
    {
        base.Init();
    }

    public override void OnEntityCreate(EntityBase entity)
    {

    }

    public override void OnEntityDestroy(EntityBase entity)
    {

    }

    public override void ClearAfter(int frame)
    {
        throw new NotImplementedException();
    }

    public override void ClearBefore(int frame)
    {
        throw new NotImplementedException();
    }

    public override MomentComponentBase GetRecord(int id, int frame)
    {
        throw new NotImplementedException();
    }

    public override void PrintRecord(int id)
    {
        throw new NotImplementedException();
    }

    public override void Record(int frame)
    {
        throw new NotImplementedException();
    }

    public override void RevertToFrame(int frame)
    {
        throw new NotImplementedException();
    }
}
