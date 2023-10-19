using ClassLibrary.Entities;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface IBookReservationRepository : IGenericRepository<BookReservation>
    {
        Task<int> GetHighestQueue(int bookId, int libraryId);
    }
}
