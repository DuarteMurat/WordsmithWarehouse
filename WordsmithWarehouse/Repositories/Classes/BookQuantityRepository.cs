using ClassLibrary.Entities;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<BookQuantity> GetQuantityByIdsAsync(int bookId, int libraryId)
        {
            return await _context.BookQuantity.Where(bq => bq.BookId == bookId && bq.LibraryId == libraryId).FirstOrDefaultAsync();
        }
    }
}
