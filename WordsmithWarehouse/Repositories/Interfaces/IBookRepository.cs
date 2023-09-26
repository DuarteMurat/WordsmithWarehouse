using ClassLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        List<Book> GetBooksList();

        string GetBookIds(List<Book> books);

        Task<List<Book>> GetBooksFromString(string source);
    }
}
