using System.Threading.Tasks;

namespace Pvirtech.Services
{
    public interface IHttpService
    {
        Task<string> Post(string url,object param);
    }
}
