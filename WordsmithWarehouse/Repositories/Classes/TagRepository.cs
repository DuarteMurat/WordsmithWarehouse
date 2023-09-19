using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Models;
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

        public async Task CreateBookTags(Book book, List<Tag> TagsFromModel)
        {
            if (TagsFromModel == null)
            {
                return;
            }

            foreach (var tag in TagsFromModel) // cycle all tags
            {
                if (await BookTagExists(book, tag)) // verify that the bookTag that references both book and tag exist
                {
                    if (!tag.isActive) // if the tag has been turned off
                    {
                        var bookTag = await _context.BookTags.FirstOrDefaultAsync(bt => bt.BookId == book.Id && bt.TagId == tag.Id);

                        _context.BookTags.Remove(bookTag);
                    }
                    // if the tag is active, and already exists in the db, do nothing.
                }
                else // if the booktag does not exist
                {
                    if (tag.isActive) // and the tag is active, create the tag
                    {
                        var tagToUse = GetTagByName(tag.Name);
                        tagToUse.isActive = true;
                        var bookTag = new BookTags
                        {
                            BookId = book.Id,
                            Tag = tagToUse,
                        };
                        tagToUse.isActive = false;

                        await _context.BookTags.AddAsync(bookTag);
                    }
                    // if it's not active, do nothing and proceed.
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

        public async Task<List<BookTags>> GetActiveBookTags(Book book)
        {
            var bookTags = _context.BookTags.Where(bt => bt.BookId == book.Id).ToList();

            foreach (var booktag in bookTags)
            {
                booktag.Tag = await GetByIdAsync(booktag.TagId);
            }

            return bookTags;
        }

        public List<Tag> GetActiveTags(BookViewModel model)
        {
            var tags = GetTagsList();
            var tagsToUse = new List<Tag>();

            foreach (var tag in tags)
            {
                tagsToUse.Add(tag);
            }

            for (int i = 0; i < model.bookTags.Count; i++)
            {
                foreach (var tag in tagsToUse)
                {
                    if (tag.Id == model.bookTags[i].TagId)
                    {
                        tag.isActive = true;
                    }
                }
            }

            return tagsToUse;
        }

        //public List<Tag> GetActiveTags(SearchBookViewModel model)
        //{
            
        //}

        public async Task<bool> BookTagExists(Book book, Tag tag)
        {
            if (await _context.BookTags.AnyAsync(bt => bt.BookId == book.Id && bt.TagId == tag.Id))
            {
                return true;
            }
            else return false;
        }

    }
}
