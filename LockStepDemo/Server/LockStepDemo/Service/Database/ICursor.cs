using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase
{
    /// <summary>
    /// 数据集读取游标接口类
    /// </summary>
    public interface ICursor
    {
        /// <summary>
        /// 读取数据集中的下一条数据
        /// </summary>
        /// <returns>True：读取下一条数据成功，False：读取下一条数据失败</returns>
        bool MoveToNext();

        /// <summary>
        /// 获取数据集的字段数量
        /// </summary>
        /// <returns>字段数量</returns>
        int FieldCount();

        /// <summary>
        /// 关闭数据集
        /// </summary>
        void Close();

        /// <summary>
        /// 得到指定字段的数据类型
        /// </summary>
        /// <param name="i">指定字段索引</param>
        /// <returns>数据类型名称</returns>
        string GetDataTypeName(int i);

        /// <summary>
        /// 得到指定字段名称
        /// </summary>
        /// <param name="i">指定字段的索引</param>
        /// <returns>字段名称</returns>
        string GetName(int i);

        /// <summary>
        /// 判断指定字段值是否为NULL
        /// </summary>
        /// <param name="i">指定字段的索引</param>
        /// <returns>True：字段值为NULL，False：字段值不为NULL</returns>
        bool IsDBNull(int i);

        /// <summary>
        /// 通过指定字段名称获取对应索引值
        /// </summary>
        /// <param name="field">指定字段名称</param>
        /// <returns>字段索引值</returns>
        int GetFieldIndex(string field);

        /// <summary>
        /// 通过指定字段索引获取DateTime类型字段的值
        /// </summary>
        /// <param name="i">指定字段索引</param>
        /// <returns>字段值</returns>
        DateTime GetDateTime(int i);

        /// <summary>
        /// 通过指定字段名称获取DateTime类型字段的值
        /// </summary>
        /// <param name="column">指定字段名称</param>
        /// <returns>字段值</returns>
        DateTime GetDateTime(string column);

        /// <summary>
        /// 通过指定字段索引获取Int类型字段的值
        /// </summary>
        /// <param name="i">指定字段索引</param>
        /// <returns>字段值</returns>
        int GetInt(int i);

        /// <summary>
        /// 通过指定字段名称获取Int类型字段的值
        /// </summary>
        /// <param name="column">指定字段名称</param>
        /// <returns>字段值</returns>
        int GetInt(string column);

        /// <summary>
        /// 通过指定字段索引获取String类型字段的值
        /// </summary>
        /// <param name="i">指定字段索引</param>
        /// <returns>字段值</returns>
        string GetString(int i);

        /// <summary>
        /// 通过指定字段名称获取String类型字段的值
        /// </summary>
        /// <param name="column">指定字段名称</param>
        /// <returns>字段值</returns>
        string GetString(string column);

        /// <summary>
        /// 通过指定字段索引获取Double类型字段的值
        /// </summary>
        /// <param name="i">指定字段索引</param>
        /// <returns>字段值</returns>
        double GetDouble(int i);

        /// <summary>
        /// 通过指定字段名称获取Double类型字段的值
        /// </summary>
        /// <param name="column">指定字段名称</param>
        /// <returns>字段值</returns>
        double GetDouble(string column);
    }
}
