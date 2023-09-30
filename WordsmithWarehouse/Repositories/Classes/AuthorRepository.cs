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
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            return author;
        }

        public IEnumerable<SelectListItem> GetComboAuthors()
        {
            var list = _context.Authors.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString(),
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select an author",
                Value = "0",
            });

            return list;
        }
    }
}
