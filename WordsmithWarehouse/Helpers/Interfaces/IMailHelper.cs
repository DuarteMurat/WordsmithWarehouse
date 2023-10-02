using WordsmithWarehouse.Helpers.Classes;

namespace WordsmithWarehouse.Helpers.Interfaces
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}
