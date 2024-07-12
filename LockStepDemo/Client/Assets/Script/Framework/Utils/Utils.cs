using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using System.Security.Cryptography;

namespace HDJ.Framework.Utils
{
    public class Utils
    {

        /// <summary>
        /// 使用特点的概率，随机结果
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static bool RandomProbability(float probability)
        {
            if (probability <= 0)
                return false;
            if (probability >= 1f)
                return true;

            int tempProbab = (int)(probability * 100);

            int getNum = UnityEngine.Random.Range(0, 100);

            if (getNum <= tempProbab)
                return true;
            else
                return false;

        }
      

        /// <summary>
        /// 获取List数据中类变量相同时的一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="fieldName">属性名字</param>
        /// <param name="fieldValue">属性值</param>
        /// <returns></returns>
        public static T GetItemFromFieldValue<T>(List<T> list, string fieldName, string fieldValue)
        {
            FieldInfo[] fields = typeof(T).GetFields();
            FieldInfo f = null;
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == fieldName)
                    f = fields[i];
            }

            if (f == null)
                return default(T);

            for (int i = 0; i < list.Count; i++)
            {
                if (f.GetValue(list[i]).ToString() == fieldValue)
                    return list[i];
            }
            return default(T);
        }


        /// <summary> 
        /// 是否能 Ping 通指定的主机  
        /// </summary>  
        /// <param name="ip">ip 地址或主机名或域名</param>  
        /// <returns>true 通，false 不通</returns>  
        public static  bool Ping(string ip)
        {
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
            options.DontFragment = true;
            string data = "Test Data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000; // Timeout 时间，单位：毫秒  
            System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
           // System.Net.NetworkInformation.PingReply reply = p.Send(ip);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                return true;
            else
                return false;
        }

       

    }
}

