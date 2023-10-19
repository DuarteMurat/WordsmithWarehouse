using ClassLibrary.Entities;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IBookQuantityRepository : IGenericRepository<BookQuantity>
    {
        Task<BookQuantity> GetQuantityByIdsAsync(int bookId, int libraryId);
    }
}
