using ClassLibrary.Entities;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class BookQuantityRepository : GenericRepository<BookQuantity>, IBookQuantityRepository
    {

        private readonly DataContext _context;

        public BookQuantityRepository(DataContext context) :base(context)
        {
            _context = context;
        }
    }
}
