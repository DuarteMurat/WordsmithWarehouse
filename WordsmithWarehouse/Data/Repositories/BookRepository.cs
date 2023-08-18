using ClassLibrary.Entities;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using WordsmithWarehouse.Interfaces.Repositories;

namespace WordsmithWarehouse.Data.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
