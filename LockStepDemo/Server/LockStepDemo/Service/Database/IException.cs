using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase
{
    interface IException
    {
        /// <summary>
        /// 获取数据库操作错误代码
        /// </summary>
        /// <returns>错误代码</returns>
        int GetErrorCode();

        /// <summary>
        /// 获取数据库操作错误信息
        /// </summary>
        /// <returns>错误信息</returns>
        string GetErrorMsg();
    }
}
