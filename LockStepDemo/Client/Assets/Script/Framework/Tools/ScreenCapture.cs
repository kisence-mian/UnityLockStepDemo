using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace HDJ.Framework.Tools
{
    public class ScreenCapture : MonoSingleton<ScreenCapture>
    {

        //  private   string  saveFilePath =Application.persistentDataPath + "/screencapture.png";

        public void CaptureScreenshot(string savePath, CallBack<string> callBack)
        {

            StartCoroutine(GetCapture(savePath, callBack));
        }

        IEnumerator GetCapture(string saveFilePath, CallBack<string> callBack)
        {
            yield return new WaitForEndOfFrame();
            int width = Screen.width;

            int height = Screen.height;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);
            yield return tex;
            byte[] imagebytes = tex.EncodeToPNG();//转化为png图
            tex.Compress(false);//对屏幕缓存进行压缩


            // image.mainTexture = tex;//对屏幕缓存进行显示（缩略图）
            File.WriteAllBytes(saveFilePath, imagebytes);//存储png图
            yield return new WaitForEndOfFrame();
            if (callBack != null)
            {
                callBack(saveFilePath);
            }

        }
    }
}
