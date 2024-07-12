using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace HDJ.Framework.Utils
{
    public class FileUtils
    {

        /// <summary>
        /// 复制一个目录下的所有文件到另一个目录
        /// </summary>
        /// <param name="oldDirectory"></param>
        /// <param name="newDirectory"></param>
        /// <param name="overwrite">是否覆盖</param>
        public static void CopyDirectory(string oldDirectory, string newDirectory, bool overwrite = true)
        {
            string[] pathArray = PathUtils.GetDirectoryFilePath(oldDirectory);
            for (int i = 0; i < pathArray.Length; i++)
            {
                string newPath = pathArray[i].Replace(oldDirectory, newDirectory);
                string s = Path.GetDirectoryName(newPath);
                if (!Directory.Exists(s))
                {
                    Directory.CreateDirectory(s);
                }
                File.Copy(pathArray[i], newPath, overwrite);
            }
        }

        public static void MoveFile(string oldFilePath, string newFilePath, bool overwrite = true)
        {
            if (!File.Exists(oldFilePath) || oldFilePath == newFilePath)
                return;
            string s = Path.GetDirectoryName(newFilePath);
            if (!Directory.Exists(s))
            {
                Directory.CreateDirectory(s);
            }
            File.Copy(oldFilePath, newFilePath, overwrite);
            DeleteFile(oldFilePath);
        }
        public static string LoadTextFileByPath(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("path dont exists ! : " + path);
                return "";
            }

            StreamReader sr = File.OpenText(path);
            StringBuilder line = new StringBuilder();
            string tmp = "";
            while ((tmp = sr.ReadLine()) != null)
            {
                line.Append(tmp);
            }

            sr.Close();
            sr.Dispose();

            return line.ToString();

        }
        public static bool DeleteFile(string path)
        {
            FileInfo t = new FileInfo(path);
            try
            {
                if (t.Exists)
                {
                    t.Delete();
                }

                Debug.Log("File Delete: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError("File delete fail: " + path + "  ---:" + e);
                return false;
            }

            return true;
        }
        /// <summary>
        /// 保存文件数据
        /// </summary>
        /// <param name="path">全路径</param>
        /// <param name="_data">数据</param>
        /// <returns></returns>
        public static bool CreateTextFile(string path, string _data)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            string temp = Path.GetDirectoryName(path);
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            FileInfo t = new FileInfo(path);
            try
            {
                if (t.Exists)
                {
                    t.Delete();
                }

                FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
                byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(_data);

                stream.Write(dataByte, 0, dataByte.Length);
                stream.Close();

                Debug.Log("File written: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError("File written fail: " + path + "  ---:" + e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5(string filePath)
        {
            try
            {
                FileInfo fileTmp = new FileInfo(filePath);
                if (fileTmp.Exists)
                {
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    int len = (int)fs.Length;
                    byte[] data = new byte[len];
                    fs.Close();

                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] result = md5.ComputeHash(data);
                    string fileMD5 = "";
                    foreach (byte b in result)
                    {
                        fileMD5 += Convert.ToString(b, 16);
                    }
                    if (!string.IsNullOrEmpty(fileMD5))
                    {
                        return fileMD5;
                    }
                }
                return "";
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

        private static GameObject gm;
        private static MonoBehaviour mono;

        /// <summary>
        /// 异步加载Txt文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public static void LoadTxtFileAsync(string path, CallBack<string> callback)
        {
            if (mono == null)
            {
                gm = new GameObject("[LoadTxtFileAsync]");
                mono = gm.AddComponent<UserFunctionBehaviour>();
            }

            if (mono)
            {
                mono.StartCoroutine(LoadTxtFileIEnumerator(path, callback));
            }
        }
        public static IEnumerator LoadTxtFileIEnumerator(string path, CallBack<string> callback)
        {

            WWW www = new WWW(path);
            yield return www;

            string data = "";
            if (string.IsNullOrEmpty(www.error))
            {
                data = www.text;
            }
            if (callback != null)
                callback(data);
            yield return new WaitForEndOfFrame();

        }
    }
}
