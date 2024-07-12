using UnityEngine;
using System.Collections;


[System.Reflection.Obfuscation(Exclude = true)]
public class ShowFPS : MonoSingleton<ShowFPS> {

    // Update is called once per frame
    void Update()
    {
        UpdateTick();
    }

    void OnGUI()
    {
        DrawFps();
    }

    private Color guiColor;
    private void DrawFps()
    {
        if (mLastFps > 50)
        {
            guiColor = new Color(0, 1, 0);
        }
        else if (mLastFps > 40)
        {
            guiColor = new Color(1, 1, 0);
        }
        else
        {
            guiColor = new Color(1.0f, 0, 0);
        }
        GUIStyle GUI_style = new GUIStyle();
        GUI_style.fontSize = 28;
        GUI_style.normal.background = null;    //这是设置背景填充的
        GUI_style.normal.textColor = guiColor;   //设置字体颜色的
        GUI_style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(50, 32, 64, 24), "fps: " + mLastFps, GUI_style);

    }

    private long mFrameCount = 0;
    private long mLastFrameTime = 0;
    static long mLastFps = 0;
    private void UpdateTick()
    {
        if (true)
        {
            mFrameCount++;
            long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
            if (mLastFrameTime == 0)
            {
                mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
            }

            if ((nCurTime - mLastFrameTime) >= 1000)
            {
                long fps = (long)(mFrameCount * 1.0f / ((nCurTime - mLastFrameTime) / 1000.0f));

                mLastFps = fps;

                mFrameCount = 0;

                mLastFrameTime = nCurTime;
            }
        }
    }
    public static long TickToMilliSec(long tick)
    {
        return tick / (10 * 1000);
    }
}
