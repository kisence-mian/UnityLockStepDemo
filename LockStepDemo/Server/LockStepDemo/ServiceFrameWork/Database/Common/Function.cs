using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase.Common
{
    class Function
    {
        /// <summary>
        /// 把SQL参数和SQL语句进行合并
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="bindArgs">SQL参数</param>
        /// <returns>完整的SQL语句</returns>
        public static string CompleteArgsToSql(string sql, string[] bindArgs)
        {
            for (int i = 0; i < bindArgs.Length; i++)
            {
                int index = sql.IndexOf("?");
                sql = sql.Insert(index, bindArgs[i]).Remove(index + bindArgs[i].Length, 1);
            }

            return sql;
        }
    }
}
