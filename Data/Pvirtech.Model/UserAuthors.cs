using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public partial class UserAuthors
    {
        public UserAuthors() { }
        /// <summary>
        /// 权限名
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 权限子集
        /// </summary>
        public List<UserAuthors> Child{get;set;}
    }
}
