using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    /// <summary>
    /// 分页对象
    /// 该对象为服务端返回对象，为方便序列化，请不要修改字段名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T>
    {
        /**当前页*/
        public int cunrrentPage;

        /** 每页条数*/
        public int pageSize;

        /**总页数*/
        public int pageCount;

        /** 总条数*/
        public int itemCount;

        /** 返回对象 */
        public List<T> bean; 
    }
}
