using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public class UserInfoItem
    {
        /**人员编号*/
        private string userNo;

        public string UserNo
        {
            get { return userNo; }
            set { userNo = value; }
        }

        /**人员姓名*/
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /**电话号码*/
        private string tel;

        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        /**类型（职责）*/
        private int userType;

        public int UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        /**备注*/
        private string userRemark;

        public string UserRemark
        {
            get { return userRemark; }
            set { userRemark = value; }
        }

        /**用户信息*/
        private string userInfo;

        public string UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; }
        }
    }
}
