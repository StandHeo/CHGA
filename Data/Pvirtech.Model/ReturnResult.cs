using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public ResultCode Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public T Model { get; set; }
    }

    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCCESS=0,
        /// <summary>
        /// 失败
        /// </summary>
        FAILE=900,
        /// <summary>
        /// 其他错误
        /// </summary>
        OTHER=909,
        /// <summary>
        /// 异常
        /// </summary>
        EXCEPTION=910
    }
}
