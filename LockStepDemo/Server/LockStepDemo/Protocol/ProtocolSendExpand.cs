using LockStepDemo.Service;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockStepDemo
{
    static class SessionSendExpand
    {
        public const int TYPE_string = 1;
        public const int TYPE_int32 = 2;
        public const int TYPE_double = 3;
        public const int TYPE_bool = 4;
        public const int TYPE_custom = 5;

        public const int TYPE_int8 = 6;
        public const int TYPE_int16 = 7;
        public const int RT_repeated = 1;
        public const int RT_equired = 0;

        public static void Send(this SyncSession session,ProtocolRequestBase msg)
        {
            ByteArray ba = new ByteArray();
            ba.bytes.AddRange(GetSendByte(msg.Key, msg.m_data));

            byte[] buffer = ba.Buffer;

            session.Send(buffer, 0, buffer.Length);
        }

        public static void SendMsg(this SyncSession session,string key ,Dictionary<string, object> data)
        {
            ByteArray ba = new ByteArray();

            List<byte> message = GetSendByte(key, data);

            int len = 3 + message.Count;
            int method = GetMethodIndex(key);

            ba.WriteShort(len);
            ba.WriteByte((byte)(method / 100));
            ba.WriteShort(method);

            if(message != null)
            {
                ba.bytes.AddRange(message);
            }
            else
            {
                ba.WriteInt(0);
            }

            byte[] buffer = ba.Buffer;
            session.Send(buffer, 0, buffer.Length);
        }


        #region 发包

        static List<byte> GetSendByte(string messageType, Dictionary<string, object> data)
    {
        try
        {
            string messageTypeTemp = "m_" + messageType + "_c";
            if (!ProtocolData.ProtocolInfo.ContainsKey(messageTypeTemp))
            {
                throw new Exception("ProtocolInfo NOT Exist ->" + messageTypeTemp + "<-");
            }

            return GetCustomTypeByte(messageTypeTemp, data);
        }
        catch (Exception e)
        {
            throw new Exception(@"ProtocolService GetSendByte Excepiton messageType is ->" + messageType
                + "<-\n" + e.ToString());
        }
    }

        static int GetStringListLength(List<object> list)
    {
        int len = 0;
        for (int i = 0; i < list.Count; i++)
        {
            byte[] bs = Encoding.UTF8.GetBytes((string)list[i]);
            len = len + bs.Length;

        }
        return len;
    }

        static List<List<byte>> m_arrayCache = new List<List<byte>>();
        static int GetCustomListLength(string customType, List<object> list)
    {
        m_arrayCache.Clear();
        int len = 0;
        for (int i = 0; i < list.Count; i++)
        {
            List<byte> bs = GetCustomTypeByte(customType, (Dictionary<string, object>)list[i]);
            m_arrayCache.Add(bs);
            len = len + bs.Count + 4;
        }
        return len;
    }

        static List<byte> GetCustomTypeByte(string customType, Dictionary<string, object> data)
    {
        string fieldName = null;
        int fieldType = 0;
        int repeatType = 0;

        try
        {
            ByteArray Bytes = new ByteArray();
            //ByteArray Bytes = new ByteArray();
            Bytes.clear();

            if (!ProtocolData.ProtocolInfo.ContainsKey(customType))
            {
                throw new Exception("ProtocolInfo NOT Exist ->" + customType + "<-");
            }

            List<Dictionary<string, object>> tableInfo = ProtocolData.ProtocolInfo[customType];

            for (int i = 0; i < tableInfo.Count; i++)
            {
                Dictionary<string, object> currentField = tableInfo[i];
                fieldType = (int)currentField["type"];
                fieldName = (string)currentField["name"];
                repeatType = (int)currentField["spl"];

                if (fieldType == TYPE_string)
                {
                    if (data.ContainsKey(fieldName))
                    {
                        if (repeatType == RT_equired)
                        {
                            Bytes.WriteString((string)data[fieldName]);
                        }
                        else
                        {
                            List<object> list = (List<object>)data[fieldName];

                            Bytes.WriteShort(list.Count);
                            Bytes.WriteInt(GetStringListLength(list));
                            for (int i2 = 0; i2 < list.Count; i2++)
                            {
                                Bytes.WriteString((string)list[i2]);
                            }
                        }
                    }
                    else
                    {
                        Bytes.WriteShort(0);
                    }
                }
                else if (fieldType == TYPE_bool)
                {
                    if (data.ContainsKey(fieldName))
                    {
                        if (repeatType == RT_equired)
                        {
                            Bytes.WriteBoolean((bool)data[fieldName]);
                        }
                        else
                        {
                            List<object> tb = (List<object>)data[fieldName];
                            Bytes.WriteShort(tb.Count);
                            Bytes.WriteInt(tb.Count);
                            for (int i2 = 0; i2 < tb.Count; i2++)
                            {
                                Bytes.WriteBoolean((bool)tb[i2]);
                            }
                        }
                    }
                }
                else if (fieldType == TYPE_double)
                {
                    if (data.ContainsKey(fieldName))
                    {
                        if (repeatType == RT_equired)
                        {
                            Bytes.WriteDouble((float)data[fieldName]);
                        }
                        else
                        {
                            List<object> tb = (List<object>)data[fieldName];
                            Bytes.WriteShort(tb.Count);
                            Bytes.WriteInt(tb.Count * 8);
                            for (int j = 0; j < tb.Count; j++)
                            {
                                Bytes.WriteDouble((float)tb[j]);
                            }
                        }
                    }
                }
                else if (fieldType == TYPE_int32)
                {
                    if (data.ContainsKey(fieldName))
                    {
                        if (repeatType == RT_equired)
                        {
                            Bytes.WriteInt(int.Parse(data[fieldName].ToString()));
                        }
                        else
                        {
                            List<object> tb = (List<object>)data[fieldName];
                            Bytes.WriteShort(tb.Count);
                            Bytes.WriteInt(tb.Count * 4);
                            for (int i2 = 0; i2 < tb.Count; i2++)
                            {
                                Bytes.WriteInt(int.Parse(tb[i2].ToString()));
                            }
                        }
                    }
                }
                else if (fieldType == TYPE_int16)
                {
                    if (data.ContainsKey(fieldName))
                    {
                        if (repeatType == RT_equired)
                        {
                            Bytes.WriteShort(int.Parse(data[fieldName].ToString()));
                        }
                        else
                        {
                            List<object> tb = (List<object>)data[fieldName];
                            Bytes.WriteShort(tb.Count);
                            Bytes.WriteInt(tb.Count * 2);
                            for (int i2 = 0; i2 < tb.Count; i2++)
                            {
                                Bytes.WriteShort(int.Parse(tb[i2].ToString()));
                            }
                        }
                    }
                }
                else if (fieldType == TYPE_int8)
                {
                    if (data.ContainsKey(fieldName))
                    {
                        if (repeatType == RT_equired)
                        {
                            Bytes.WriteInt8(int.Parse(data[fieldName].ToString()));
                        }
                        else
                        {
                            List<object> tb = (List<object>)data[fieldName];
                            Bytes.WriteShort(tb.Count);
                            Bytes.WriteInt(tb.Count);
                            for (int i2 = 0; i2 < tb.Count; i2++)
                            {
                                Bytes.WriteInt8(int.Parse(tb[i2].ToString()));
                            }
                        }
                    }
                }
                else
                {
                    if (data.ContainsKey(fieldName))
                    {
                        if (repeatType == RT_equired)
                        {
                            customType = (string)currentField["vp"];
                            Bytes.bytes.AddRange(GetSendByte(customType, (Dictionary<string, object>)data[fieldName]));
                        }
                        else
                        {
                            List<object> tb = (List<object>)data[fieldName];

                            Bytes.WriteShort(tb.Count);
                            //这里会修改m_arrayCatch的值，下面就可以直接使用
                            Bytes.WriteInt(GetCustomListLength(customType, tb));

                            for (int j = 0; j < m_arrayCache.Count; j++)
                            {
                                List<byte> tempb = m_arrayCache[j];
                                Bytes.WriteInt(tempb.Count);
                                Bytes.bytes.AddRange(tempb);
                            }
                        }
                    }
                }
            }
            return Bytes.bytes;
        }
        catch (Exception e)
        {
            throw new Exception(@"GetCustomTypeByte Excepiton CustomType is ->" + customType
               + "<-\nFieldName:->" + fieldName
               + "<-\nFieldType:->" + GetFieldType(fieldType)
               + "<-\nRepeatType:->" + GetRepeatType(repeatType)
               + "<-\nCustomType:->" + customType
               + "<-\n" + e.ToString());
        }
    }

        static string GetFieldType(int fieldType)
    {
        switch (fieldType)
        {
            case TYPE_string: return "TYPE_string";
            case TYPE_int32: return "TYPE_int32";
            case TYPE_int16: return "TYPE_int16";
            case TYPE_int8: return "TYPE_int8";
            case TYPE_double: return "TYPE_double";
            case TYPE_bool: return "TYPE_bool";
            case TYPE_custom: return "TYPE_custom";
            default: return "Error";
        }
    }

    static string GetRepeatType(int repeatType)
    {
        switch (repeatType)
        {
            case RT_repeated: return "RT_repeated";
            case RT_equired: return "RT_equired";
            default: return "Error";
        }
    }

      static  int GetMethodIndex(string messageType)
        {
            try
            {
                return ProtocolData.MethodIndexInfo[messageType];
            }
            catch
            {
                throw new Exception("GetMethodIndex ERROR! NOT Find " + messageType);
            }
        }

        #endregion
    }
}
