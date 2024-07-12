using UnityEngine;
using System.Collections;
namespace HDJ.Framework.Tools
{

    public class Timer
    {
        private string name = "";
        public string Name
        {
            get { return name; }
        }
        private TimerPlayState timerState = TimerPlayState.Stop;
        public TimerPlayState TimerState
        {
            get { return timerState; }
        }

        public TimerRepeatMode repeatMode = TimerRepeatMode.Loop;
        /// <summary>
        /// 重复调用次数
        /// </summary>
        public int repeatCount = 1;
        /// <summary>
        /// 重复调用时的时间间隔
        /// </summary>
        public float repeatTime = 0.1f;
        /// <summary>
        /// 每计时完成一次回调方法，参数：还剩余多少次计时,-1无限次
        /// </summary>
        public CallBack<int> OnEveryRepeatComplete = null;
        /// <summary>
        /// 计时完成回调方法,有限计时次数TimerRepeatMode.Times时才回调
        /// </summary>
        public CallBack<string> OnComplete = null;
        /// <summary>
        /// 每次Update回调，参数：这次计时还剩余多少时间
        /// </summary>
        public CallBack<float> OnUpdate = null;

        /// <summary>
        /// 是否使用真实时间，使用真实时间不受时间缩放的影响
        /// </summary>
        public bool isRealTime = false;
        /// <summary>
        /// 当计时完成时自动销毁（）
        /// </summary>
        public bool autoDestroyOnStop = false;


        public Timer(string name, float repeatTime, TimerRepeatMode repeatMode, int repeatCount = 1)
        {
            this.name = name;
            this.repeatTime = repeatTime;
            this.repeatMode = repeatMode;
            this.repeatCount = repeatCount;
            Start();
        }
        private float runTimeCount = 0f;
        private int currentRepeatCount = 0;
        public void RunUpdate()
        {
            if (timerState == TimerPlayState.Playing)
            {
                TimeUtils.UpdateRunTimerDelayFunction(ref runTimeCount, repeatTime, isRealTime, EveryRepeatComplete);
                if (OnUpdate != null)
                    OnUpdate(runTimeCount);
            }
        }
        private void EveryRepeatComplete()
        {
            if (repeatMode == TimerRepeatMode.Times)
            {
                currentRepeatCount--;
                if (currentRepeatCount <= 0)
                {
                    timerState = TimerPlayState.Stop;
                    if (OnComplete != null)
                        OnComplete(name);
                }
            }
            if (OnEveryRepeatComplete != null)
                OnEveryRepeatComplete(currentRepeatCount);

        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            if (timerState == TimerPlayState.Stop)
            {
                currentRepeatCount = repeatCount;
                runTimeCount = repeatTime;
            }
            timerState = TimerPlayState.Playing;
        }
        public void Pause()
        {
            if (timerState == TimerPlayState.Playing)
                timerState = TimerPlayState.Pause;
        }

        public void Stop()
        {
            timerState = TimerPlayState.Stop;
        }

    }


    /// <summary>
    /// 计时器的重复模式
    /// </summary>
    public enum TimerRepeatMode
    {
        /// <summary>
        /// 有限重复次数
        /// </summary>
        Times,
        /// <summary>
        /// 无限重复次数
        /// </summary>
        Loop,
    }
    public enum TimerPlayState
    {
        Playing,
        Pause,
        Stop,
    }
}
