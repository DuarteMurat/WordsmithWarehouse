using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Classes;

namespace WordsmithWarehouse.Helpers.Interfaces
{
    public interface IMailHelper
    {
        Task<Response> SendEmail(string to, string subject, string body);
    }
}
