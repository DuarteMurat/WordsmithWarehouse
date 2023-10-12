using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<SelectListItem> GetComboLibraries()
        {
            var list = _context.Libraries.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString(),
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a library",
                Value = "0",
            });

            return list;
        }
    }
}
