using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace HDJ.Framework.Tools
{
    public class TimerManager : MonoBehaviour
    {
        private static Dictionary<string, Timer> timerDic = new Dictionary<string, Timer>();
        private static List<Timer> removeTimerList = new List<Timer>();
        private static TimerManager instance;
        private static void Init()
        {
            if (instance == null)
            {
                instance = OtherUtils.CreateMonoBehaviourBase<TimerManager>("[TimerManager]");
            }
        }

        public static Timer GetTimer(string name)
        {
            if (timerDic.ContainsKey(name))
                return timerDic[name];
            return null;
        }
        public static void Clear()
        {
            timerDic.Clear();
        }
        /// <summary>
        /// 重复次数没有限制的Timer
        /// </summary>
        /// <param name="repeatTime">重复调用时的时间间隔</param>
        /// <param name="name">计时器名字</param>
        /// <returns></returns>
        public static Timer SetTimerNoLimitRepeat(float repeatTime, string name = "")
        {
            return SetTimer(repeatTime, TimerRepeatMode.Loop, -1, name);
        }

        /// <summary>
        /// 有限的重复次数的Timer，默认计时完成后会自动销毁，不销毁请使用autoDestroyOnStop
        /// </summary>
        /// <param name="repeatTime">重复调用时的时间间隔</param>
        /// <param name="repeatCount">重复次数</param>
        ///  <param name="name">计时器名字</param>
        /// <returns></returns>
        public static Timer SetTimerLimitRepeat(float repeatTime, int repeatCount = 1, string name = "")
        {
            Timer t = SetTimer(repeatTime, TimerRepeatMode.Times, repeatCount, name);
            t.autoDestroyOnStop = true;
            return t;
        }
        /// <summary>
        /// 只运行一次自动销毁的计时器
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="OnComplete">计时完成回调，参数：计时器名字</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Timer SetTimerRunOnce(float delay, string name = "", CallBack<string> OnComplete = null)
        {
            Timer t = SetTimerLimitRepeat(delay, 1, name);
            t.OnComplete = OnComplete;
            return t;
        }

        public static void DestroyTimer(string name)
        {
            if (timerDic.ContainsKey(name))
            {
                timerDic[name].autoDestroyOnStop = true;
                timerDic[name].Stop();
            }
        }

        /// <summary>
        /// 自定义Timer
        /// </summary>
        /// <param name="delayOfFirstRun">间隔delayOfFirstRun秒第一次调用TimerCallBack（）,如不想让第一次调用的间隔时间和后面的不同，请将delayOfFirstRun和repeatTime设置的值不一致</param>
        /// <param name="repeatTime">重复调用时的时间间隔</param>
        /// <param name="repeatMode">计时器的重复模式，有限和无限重复</param>
        /// <param name="repeatCount">重复次数</param>
        /// <returns></returns>
        private static Timer SetTimer(float repeatTime, TimerRepeatMode repeatMode, int repeatCount = 1, string name = "")
        {
            Init();
            string tempName = name;
            if (string.IsNullOrEmpty(tempName))
            {
                tempName = Time.realtimeSinceStartup.ToString();

            }
            Timer timer = new Timer(tempName, repeatTime, repeatMode, repeatCount);
            timerDic.Add(tempName, timer);
            return timer;
        }

        List<Timer> tempList = new List<Timer>();
        void Update()
        {
            tempList.Clear();
            tempList.AddRange(timerDic.Values);
            for (int i = 0; i < tempList.Count; i++)
            {
                tempList[i].RunUpdate();
                if (tempList[i].TimerState == TimerPlayState.Stop && tempList[i].autoDestroyOnStop)
                    removeTimerList.Add(tempList[i]);
            }

            if (removeTimerList.Count > 0)
            {
                for (int j = 0; j < removeTimerList.Count; j++)
                {

                    timerDic.Remove(removeTimerList[j].Name);
                }

                removeTimerList.Clear();
            }
        }
    }

}

  

