using ClassLibrary.Entities;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class BookReservationRepository : GenericRepository<BookReservation> , IBookReservationRepository
    {
        private readonly DataContext _context;

        public BookReservationRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
