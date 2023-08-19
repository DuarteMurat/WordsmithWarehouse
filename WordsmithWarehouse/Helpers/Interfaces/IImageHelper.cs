using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Helpers.Interfaces
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
