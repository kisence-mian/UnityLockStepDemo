using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HDJ.Framework.Tools
{
    public class Stopwatch : MonoBehaviour
    {
        private static Dictionary<string, StopwatchTimeData> timerDic = new Dictionary<string, StopwatchTimeData>();
        private static Stopwatch instance;
        private static void Init()
        {
            if (instance == null)
            {
                instance = OtherUtils.CreateMonoBehaviourBase<Stopwatch>();
            }
        }

        public static StopwatchTimeData GetTimer(string name)
        {
            if (timerDic.ContainsKey(name))
                return timerDic[name];
            return null;
        }
        public static void Clear()
        {
            timerDic.Clear();
        }
        public static StopwatchTimeData Add(float callBackRate = -1, CallBack<StopwatchTimeData> callBack=null, bool isRealTime=false, string name = "")
        {
            Init();
            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString(); 
            }
            if (timerDic.ContainsKey(name))
            {
                Debug.LogError("Stopwatch Name contain " + name);
                return null;
            }
            StopwatchTimeData d = new StopwatchTimeData(name, callBackRate, isRealTime,callBack);
            timerDic.Add(name, d);
            d.Play();
            return d;
        }

        private List<StopwatchTimeData> tempList = new List<StopwatchTimeData>();
        // Update is called once per frame
        void Update()
        {
            if (timerDic.Values.Count == 0)
                return;
            tempList.Clear();
            tempList.AddRange(timerDic.Values);

            for (int i = 0; i < tempList.Count; i++)
            {
                StopwatchTimeData st = tempList[i];
                if (st.State== TimerPlayState.Stop)
                {
                    timerDic.Remove(st.Name);
                    continue;
                }

                if(st.State== TimerPlayState.Playing)
                {
                    st.RunUpdate();
                }
            }


        }

       
    }
    public class StopwatchTimeData
    {
        private string name = "";
        private float timeCount = 0;
        //callback调用频率，每隔多少秒调用，-1，位每次update时调用
        private float callBackRate = -1;
        /// <summary>
        /// 是否使用真实时间，使用真实时间不受时间缩放的影响
        /// </summary>
        private bool isRealTime = false;
        private TimerPlayState state = TimerPlayState.Stop;
        public CallBack<StopwatchTimeData> callBack;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public float TimeCount
        {
            get
            {
                return timeCount;
            }
        }

        public float CallBackRate
        {
            get
            {
                return callBackRate;
            }
        }

        public bool IsRealTime
        {
            get
            {
                return isRealTime;
            }
        }

        public TimerPlayState State
        {
            get
            {
                return state;
            }
        }

        public StopwatchTimeData(string name, float callBackRate, bool isRealTime, CallBack<StopwatchTimeData> callBack)
        {
            this.name = name;
            this.callBackRate = callBackRate;
            this.isRealTime = isRealTime;
            this.callBack = callBack;
        }
        private float tempT;
        private float tempD;
        public void RunUpdate()
        {
            if (isRealTime)
            {
                tempD = Time.unscaledDeltaTime;

            }
            else
            {
                tempD = Time.deltaTime;
            }
            timeCount += tempD;

            if (tempT <= 0)
            {
                if (callBack != null)
                {
                    callBack(this);
                }
                tempT = CallBackRate;
            }
            else
            {
                tempT -= tempD;
            }
        }

        public void Play()
        {
            if(state == TimerPlayState.Stop)
            {
                timeCount = 0;
            }
            state = TimerPlayState.Playing;
        }
        public void Pause()
        {
            if(state == TimerPlayState.Playing)
            {
                state = TimerPlayState.Pause;
            }
        }
        public void Stop()
        {
            state = TimerPlayState.Stop;
        }
    }
}
