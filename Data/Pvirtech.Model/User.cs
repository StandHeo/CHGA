using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pvirtech.Model
{
    public  class User
    {
        /// <summary>
        /// 登录警员编号
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 登录令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 登录警员名字
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 岗位名
        /// </summary>
        public string JobName { get; set; }
        public string LoginName { get; set; }
        /// <summary>
        /// 权限集
        /// </summary>
        public List<UserAuthors> Author { get; set; }
        public DateTime CurrentTime { get; set; } 
         
        public string ConnectionId { get; set; }

        public string DeptName { get; set; }
        public string DeptNo { get; set; }
    //    deptName":"成华区分局装备财务科","deptNo":"510108180000","
          public bool IsAllFunction { get; set; }
           // ,"isAllFunction":true
    }
}
