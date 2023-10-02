using ClassLibrary.Entities;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Books.Include(p => p.User);
        }

        public string GetBookIds(List<Book> books)
        {
            string ids = string.Empty;

            if (books == null) return ids;

            foreach (var book in books)
            {
                ids += book.Id.ToString() + ",";
            }

            ids = ids.Substring(0, ids.Length - 1);
            return ids;
        }

        public async Task<List<Book>> GetBooksFromString(string source)
        {
            List<Book> books = new List<Book>();

            if (string.IsNullOrEmpty(source))
                return books;

            string[] ids = source.Split(',');

            if (ids.Length <= 0) return books;

            foreach (var id in ids)
            {
                books.Add(await GetByIdAsync(int.Parse(id)));
            }

            return books;
        }

        public List<Book> GetBooksList()
        {
            var list = new List<Book>();

            list = _context.Books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Subtitle = b.Subtitle,
                IsAvailableOnline = b.IsAvailableOnline,
                ISBN = b.ISBN,
                IsAvailablePhysical = b.IsAvailablePhysical,
                AuthorId = b.AuthorId,
                ImageURL = b.ImageURL,
                Pages = b.Pages,
                Publisher = b.Publisher,
                tagIds = b.tagIds,
            }).OrderBy(t => t.Title).ToList();

            return list;
        }
    }
}
