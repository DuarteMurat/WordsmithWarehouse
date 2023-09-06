using ClassLibrary.Entities;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class BookTagsRepository: GenericRepository<BookTags>, IBookTagsRepository
    {
        private readonly DataContext _context;

        public BookTagsRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
