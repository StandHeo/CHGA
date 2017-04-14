using Pvirtech.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    /// <summary>
    /// �û��ӿ�
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// ��¼
        /// </summary>
        /// <returns></returns>
        Task<Result<User>> Login(string username, string password); 

        /// <summary>
        /// ע��
        /// </summary>
        Task Logout(string userNo, string token);

        bool GetCurrentStatus();

        void SetCurrentStatus(bool status);
        /// <summary>
        /// ������Ϣ
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
        /// ���Ͷ���
        /// </summary>
        /// <param name="title">����</param>
        /// <param name="sender">������</param>
        /// <param name="phoneNo">���ͺ���</param>
        /// <param name="content">����</param>
        /// <returns></returns>
        //Task<Result<bool>> SendPhoneMessager(List<PhoneMes> lists);

        /// <summary>
        /// �û�æµ/����
        /// </summary>
        /// <param name="userInfo">�û����</param>
        /// <param name="TypeId">״̬2���У�3:æ</param>
        /// <returns></returns>
        Task<Result<bool>> UserBusy(string userInfo,string TypeId);
    }
}