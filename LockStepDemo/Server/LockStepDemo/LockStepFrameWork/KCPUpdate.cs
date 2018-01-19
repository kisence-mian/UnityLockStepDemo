using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo.LockStepFrameWork
{

    public delegate void Update();



    class KCPUpdate
    {

        List<Update> m_list = new List<Update>();
        private static KCPUpdate uniqueInstance;


        public static KCPUpdate GetInstance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (uniqueInstance == null)
            {
                uniqueInstance = new KCPUpdate();
            }
            return uniqueInstance;
        }

        private KCPUpdate()
        {
            System.Timers.Timer t = new System.Timers.Timer(10);
            t.Elapsed += new System.Timers.ElapsedEventHandler(loop);
            t.AutoReset = true;
            t.Enabled = true;
        }


        private void loop(object source, System.Timers.ElapsedEventArgs e)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                m_list[i]();
            }
            //Debug.Log("kcp update");
        }

        public void Add(Update fun)
        {
            if (m_list.Contains(fun))
            {
                return;
            }
            m_list.Add(fun);
        }

        public void Remove(Update fun)
        {
            if (m_list.Contains(fun))
            {
                m_list.Remove(fun);
            }
        }
    }
}
