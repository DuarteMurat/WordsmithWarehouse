using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        List<Book> GetBooksList();

        string GetBookIds(List<Book> books);

        Task<List<Book>> GetBooksFromString(string source);

        public IQueryable GetAllWithUsers();

        Task<string> UploadBookFileAsync(IFormFile imageFile, string folder);
    }
}
