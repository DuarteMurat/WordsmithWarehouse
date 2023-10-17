using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<SelectListItem>> GetComboLibraries()
        {
            var list = await _context.Libraries.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString(),
            }).OrderBy(l => l.Text).ToListAsync();
            return list;
        }
    }
}
