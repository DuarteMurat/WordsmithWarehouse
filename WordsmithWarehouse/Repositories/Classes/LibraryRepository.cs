using ClassLibrary.Entities;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class LibraryRepository : GenericRepository<Library> , ILibraryRepository
    {
        private readonly DataContext _context;

        public LibraryRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
