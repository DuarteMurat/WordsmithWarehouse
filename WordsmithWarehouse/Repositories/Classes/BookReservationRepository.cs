using ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class BookReservationRepository : GenericRepository<BookReservation>, IBookReservationRepository
    {
        private readonly DataContext _context;
        public BookReservationRepository(DataContext context) :base(context)
        {
            _context = context;
        }
           
       public async Task<int> GetHighestQueue(int bookId, int libraryId)
        {
            if (await _context.BookReservations.AnyAsync( br => br.BookId == bookId && br.LibraryId == libraryId))
            {
                var reservationNumber = await _context.BookReservations
                .Where(br => br.BookId == bookId && br.LibraryId == libraryId)
                .OrderByDescending(br => br.QueueNumber)
                .FirstAsync();

                return reservationNumber.QueueNumber;
            }
            return 0;
            
        }
    }
}
