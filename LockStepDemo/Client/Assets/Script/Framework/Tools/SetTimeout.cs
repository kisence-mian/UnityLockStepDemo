using UnityEngine;
using System.Collections.Generic;
using System;
namespace HDJ.Framework.Tools
{
    public class SetTimeout : MonoBehaviour
    {
        public class CallbackInfo
        {
            public CallBack call;
            public float dely;
            public float endTime;
            public bool isUseRealTime = false;
            public bool once;
        }


        private static List<CallbackInfo> mCallbackList = new List<CallbackInfo>();
        private static List<CallbackInfo> mRemoveList = new List<CallbackInfo>();

        private static SetTimeout instance = null;

        private static void CreateInstance()
        {
            if (instance != null)
                return;
            GameObject obj = new GameObject("[SetTimeout]");
            instance = obj.AddComponent<SetTimeout>();
            DontDestroyOnLoad(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="handler"></param>
        /// <param name="useRealTime"></param>
        /// <param name="once"></param>
        public static void Add(float delay, CallBack handler, bool once = true, bool useRealTime = true)
        {
            CreateInstance();
            if (!useRealTime)
                mCallbackList.Add(new CallbackInfo() { call = handler, dely = delay, endTime = delay, isUseRealTime = false, once = once });
            else
                mCallbackList.Add(new CallbackInfo() { call = handler, dely = delay, endTime = Time.realtimeSinceStartup + delay, isUseRealTime = true, once = once });
        }

        public static void Remove(CallBack handler)
        {
            CreateInstance();
            for (int i = 0; i < mCallbackList.Count; i++)
            {
                if (mCallbackList[i].call == handler)
                {

                    mRemoveList.Add(mCallbackList[i]);
                    break;
                }
            }
        }

        public static bool Contains(CallBack handler)
        {
            CreateInstance();
            for (int i = 0; i < mCallbackList.Count; i++)
            {
                if (mCallbackList[i].call == handler)
                {
                    return true;
                }
            }
            return false;
        }

        public static void ClearData()
        {
            CreateInstance();
            mCallbackList.Clear();
        }

        private void Update()
        {
            if (mCallbackList.Count <= 0)
                return;

            for (int i = 0; i < mCallbackList.Count; i++)
            {
                if (mCallbackList[i].call == null)
                {
                    mRemoveList.Add(mCallbackList[i]);
                    continue;
                }

                if (!mCallbackList[i].isUseRealTime)
                    mCallbackList[i].endTime -= Time.deltaTime;

                if ((!mCallbackList[i].isUseRealTime && mCallbackList[i].endTime <= 0) || (mCallbackList[i].isUseRealTime && Time.realtimeSinceStartup >= mCallbackList[i].endTime))
                {
                    CallbackInfo info = mCallbackList[i];
                    if (!mCallbackList[i].once)
                    {
                        info.endTime += info.dely;
                        mCallbackList[i] = info;
                    }
                    else
                        mRemoveList.Add(mCallbackList[i]);

                    try
                    {

                        info.call();
                    }
                    catch (Exception e)
                    {
                        Debug.Log("SetTimerout Error: " + e.ToString());
                    }
                }
            }

            if (mRemoveList.Count > 0)
            {
                for (int j = 0; j < mRemoveList.Count; j++)
                {

                    mCallbackList.Remove(mRemoveList[j]);
                }

                mRemoveList.Clear();
            }
        }
    }

}



