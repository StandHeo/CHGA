using Pvirtech.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pvirtech.Services
{
    public interface ICommonRepository
    {
        /// <summary>
        /// 获取字段信息
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        Task<Result<List<DictItem>>> GetDictItem(string kind);

        List<AppModuleInfo> GetAlarmModule();
        Task<bool> SendQRCodeTimeOut(string jqlsh);
        Task<string> SendQRCodeToDld(string jqlsh, string opUserNo, string opUserName);
        Task<bool> GenerateQRCode(string jqlsh);
      
    }
}
