using Pvirtech.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        Task<Result<User>> Login(string username, string password); 

        /// <summary>
        /// 注销
        /// </summary>
        Task Logout(string userNo, string token);

        bool GetCurrentStatus();

        void SetCurrentStatus(bool status);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="UserNo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        void SendMessage(int UserNo,string message);
        void SendMessage(List<int> UserNos, string message);
        string ReceiveMessage();
        string ReceiveMessage(int UserNo, string message);
        IEnumerable<User> GetUserList();

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="sender">发送人</param>
        /// <param name="phoneNo">发送号码</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        //Task<Result<bool>> SendPhoneMessager(List<PhoneMes> lists);

        /// <summary>
        /// 用户忙碌/空闲
        /// </summary>
        /// <param name="userInfo">用户编号</param>
        /// <param name="TypeId">状态2：闲；3:忙</param>
        /// <returns></returns>
        Task<Result<bool>> UserBusy(string userInfo,string TypeId);
    }
}