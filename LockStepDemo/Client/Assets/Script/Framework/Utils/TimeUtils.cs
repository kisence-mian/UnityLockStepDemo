using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtils  {

    /// <summary>
    /// 放在Update里，间隔多少时间执行callback
    /// </summary>
    /// <param name="runTimeCount"></param>
    /// <param name="delayTime"></param>
    /// <param name="callBack"></param>
    public static void UpdateRunTimerDelayFunction(ref float runTimeCount, float delayTime,bool isRealTime =false, CallBack callBack =null)
    {
        if (runTimeCount <= 0)
        {
            runTimeCount = delayTime;
            if (callBack != null)
                callBack();
        }
        else
        {
            if (isRealTime)
            {
                runTimeCount -= Time.unscaledDeltaTime;
            }
            else
                runTimeCount -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 转换时间格式
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string GetTimeFormat(int seconds, string format)
    {
        int Hour = (int)(seconds / 3600f);
        int Minute = (int)(seconds % 3600 / 60);
        int Second = (int)(seconds % 3600 % 60);

        string tempStr = "";
        switch (format)
        {
            case "0:0:0":
                return Hour + ":" + Minute + ":" + Second;
            case "00:00:00":
                tempStr = Hour < 10 ? ("0" + Hour) : Hour.ToString();
                tempStr += ":";
                tempStr = Minute < 10 ? ("0" + Minute) : Minute.ToString();
                tempStr += ":";
                tempStr += Second < 10 ? ("0" + Second) : Second.ToString();
                return tempStr;
            case "0:0":
                return Minute + ":" + Second;
            case "00:00":
                tempStr = Minute < 10 ? ("0" + Minute) : Minute.ToString();
                tempStr += ":";
                tempStr += Second < 10 ? ("0" + Second) : Second.ToString();
                return tempStr;

            case "0-0-0":
                return Hour + "-" + Minute + "-" + Second;
            case "0-0":
                return Minute + "-" + Second;
        }

        return seconds.ToString();
    }
    /// <summary>
    /// 转换时间格式
    /// </summary>
    /// <param name="seconds100">秒*100</param>
    /// <returns></returns>
    public static string GetTimeFormat(int seconds100)
    {
        int temp = seconds100 / 100;
        // int Hour = (int)(temp / 3600f);
        int Minute = (int)(temp % 3600 / 60);
        int Second = (int)(temp % 3600 % 60);
        int dian = (int)(seconds100 - temp * 100);
        string tempStr = "";

        tempStr = Minute < 10 ? ("0" + Minute) : Minute.ToString();
        tempStr += ":";
        tempStr += Second < 10 ? ("0" + Second) : Second.ToString();
        tempStr += ".";
        tempStr += dian < 10 ? ("0" + dian) : dian.ToString();
        return tempStr;

    }

    /// <summary>
    /// 将字符串时间转化为秒（格式一定要是时，分，秒）
    /// </summary>
    /// <param name="timeValue"></param>
    /// <param name="format">格式如（01：23:45或 12/10/05）</param>
    /// <returns></returns>
    public static int StringTimeChangeTo(string timeValue, char format)
    {
        string[] values = timeValue.Split(format);
        List<string> list = new List<string>();
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] == "")
                continue;
            list.Add(values[i]);
        }

        return int.Parse(list[0]) * 3600 + int.Parse(list[1]) * 60 + int.Parse(list[2]);
    }
}
