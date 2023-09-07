using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly DataContext _context;
        public TagRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateBookTags(Book book, List<Tag> Tags)
        {
            if (Tags == null)
            {
                return;
            }

            foreach (var tag in Tags)
            {
                if (tag.isActive)
                {
                    var tagToUse = GetTagByName(tag.Name);

                    var bookTag = new BookTags
                    {
                        Book = book,
                        Tag = tagToUse,
                    };

                    await _context.BookTags.AddAsync(bookTag);
                }
            }
            await _context.SaveChangesAsync();
        }

        public List<Tag> GetTagsList()
        {
            var list = new List<Tag>();

            list = _context.Tags.Select(t => new Tag
            {
                Id = t.Id,
                Name = t.Name,
                isActive = t.isActive,
            }).OrderBy(t => t.Name).ToList();

            return list;
        }

        public Tag GetTagByName(string name)
        {
            var tag = _context.Tags.FirstOrDefault(t => t.Name == name);

            return tag;
        }
    }
}
