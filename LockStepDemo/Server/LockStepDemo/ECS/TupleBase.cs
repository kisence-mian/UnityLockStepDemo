//using System;
//using System.Collections;
//using System.Collections.Generic;

//public struct TupleBase
//{
//    public virtual List<Type> GetComponentTypeList()
//    {
//        return new List<Type>();
//    }

//    List<string> m_CompNameList = null;
//    public List<string> GetComponentNameList()
//    {
//        if(m_CompNameList == null)
//        {
//            List<Type> list = GetComponentTypeList();
//            m_CompNameList = new List<string>();
//            for (int i = 0; i < list.Count; i++)
//            {
//                m_CompNameList.Add(list[i].Name);
//            }
//        }

//        return m_CompNameList;
//    }
//}
