using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase
{
    /// <summary>
    /// 数据库操作接口类
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// 打开数据库
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭数据库
        /// </summary>
        void Close();

        /// <summary>
        /// 判断数据库是否打开
        /// </summary>
        /// <returns>True：数据库已打开，False：数据库未打开</returns>
        bool IsOpen();

        /// <summary>
        /// 修改打开的数据库
        /// </summary>
        /// <param name="database">数据库名称</param>
        void ChangeDatabase(string database);

        /// <summary>
        /// 执行一条SQL语句，
        /// </summary>
        /// <param name="sql">SQL语句，可以使用?做为占位符</param>
        /// <param name="bindArgs">用于替换占位符的数组</param>
        /// <returns>执行SQL语句影响的行数</returns>
        int ExecSQL(string sql, string[] bindArgs);

        /// <summary>
        /// 执行SQL文件
        /// </summary>
        /// <param name="info">SQL文件对象</param>
        /// <returns>执行SQL语句影响的行数</returns>
        int ExecSQL(FileInfo info);

        /// <summary>
        /// 执行一条插入语句
        /// </summary>
        /// <param name="table">数据表名称</param>
        /// <param name="nullColumnHack">插入NULL值的字段</param>
        /// <param name="values">添加的字段</param>
        /// <returns>返回新数据的主键ID</returns>
        long Insert(string table, string nullColumnHack, Dictionary<string, string> values);

        /// <summary>
        /// 执行一条更新语句
        /// </summary>
        /// <param name="table">数据表名称</param>
        /// <param name="values">新的数据值</param>
        /// <param name="whereClause">条件语句，可以使用?作为占位符</param>
        /// <param name="whereArgs">替换占位符的条件参数</param>
        /// <returns>更新数据影响的行数</returns>
        int Update(string table, Dictionary<string, string> values, string whereClause, string[] whereArgs);

        /// <summary>
        /// 执行一条删除语句
        /// </summary>
        /// <param name="table">数据表名称</param>
        /// <param name="whereClause">条件语句，可以使用?作为占位符</param>
        /// <param name="whereArgs">替换占位符的条件参数</param>
        /// <returns>删除数据影响的行数</returns>
        int Delete(string table, string whereClause, string[] whereArgs);

        /// <summary>
        /// 执行一条查询语句
        /// </summary>
        /// <param name="distinct">是否过滤重复值</param>
        /// <param name="table">数据表名称</param>
        /// <param name="columns">查询字段，如果为NULL表示所有字段</param>
        /// <param name="whereClause">条件语句，可以使用?作为占位符</param>
        /// <param name="whereArgs">替换占位符的条件参数</param>
        /// <param name="groupBy">分组语句，NULL或空字符串表示不添加该语句</param>
        /// <param name="having">分组条件，NULL或空字符串表示不添加该语句</param>
        /// <param name="orderBy">数据排序，NULL或空字符串表示不添加该语句</param>
        /// <param name="limit">限制条件，NULL或空字符串表示不添加该语句</param>
        /// <returns>数据集的游标</returns>
        ICursor Query(bool distinct, string table, string[] columns, string whereClause, string[] whereArgs, string groupBy, string having, string orderBy, string limit);

        /// <summary>
        /// 执行一条查询语句，并且不过滤重复值和限制数据集大小
        /// </summary>
        /// <param name="table">数据表名称</param>
        /// <param name="columns">查询字段，如果为NULL表示所有字段</param>
        /// <param name="whereClause">条件语句，可以使用?作为占位符</param>
        /// <param name="whereArgs">替换占位符的条件参数</param>
        /// <param name="groupBy">分组语句，NULL或空字符串表示不添加该语句</param>
        /// <param name="having">分组条件，NULL或空字符串表示不添加该语句</param>
        /// <param name="orderBy">数据排序，NULL或空字符串表示不添加该语句</param>
        /// <returns>数据集的游标</returns>
        ICursor Query(string table, string[] columns, string whereClause, string[] whereArgs, string groupBy, string having, string orderBy);

        /// <summary>
        /// 执行一条查询语句，并且不过滤重复值
        /// </summary>
        /// <param name="table">数据表名称</param>
        /// <param name="columns">查询字段，如果为NULL表示所有字段</param>
        /// <param name="whereClause">条件语句，可以使用?作为占位符</param>
        /// <param name="whereArgs">替换占位符的条件参数</param>
        /// <param name="groupBy">分组语句，NULL或空字符串表示不添加该语句</param>
        /// <param name="having">分组条件，NULL或空字符串表示不添加该语句</param>
        /// <param name="orderBy">数据排序，NULL或空字符串表示不添加该语句</param>
        /// <param name="limit">限制条件，NULL或空字符串表示不添加该语句</param>
        /// <returns>数据集的游标</returns>
        ICursor Query(string table, string[] columns, string whereClause, string[] whereArgs, string groupBy, string having, string orderBy, string limit);
    }
}
