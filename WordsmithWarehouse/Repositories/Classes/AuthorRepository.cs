using ClassLibrary.Entities;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
