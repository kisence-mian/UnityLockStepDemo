using Protocol;
using System.Collections.Generic;

public class RecordComponent<T> : SingletonComponent where T : MomentComponentBase, new()
{
    public List<T> m_record = new List<T>(); 

    public void ClearBefore(int frame)
    {
        for (int i = 0; i < m_record.Count; i++)
        {
            if(m_record[i].Frame < frame)
            {
                m_record.RemoveAt(i);
                i--;
            }
        }
    }

    public void ClearAfter(int frame)
    {
        for (int i = 0; i < m_record.Count; i++)
        {
            if (m_record[i].Frame > frame)
            {
                m_record.RemoveAt(i);
                i--;
            }
        }
    }

    List<T> list = new List<T>();
    public List<T> GetRecordList(int frame)
    {
        list.Clear();

        for (int i = 0; i < m_record.Count; i++)
        {
            if (m_record[i].Frame == frame)
            {
                list.Add(m_record[i]);
            }
        }

        return list;
    }
}
