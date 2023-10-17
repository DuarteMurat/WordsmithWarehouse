using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Controllers;
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

        /// <summary>
        /// Populates a list with all tags present in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetTagsList()
        {
            var list = new List<Tag>();

            list = await _context.Tags.Select(t => new Tag
            {
                Id = t.Id,
                Name = t.Name,
                isActive = t.isActive,
                isAdmin = t.isAdmin,
            }).OrderBy(t => t.Name).ToListAsync();

            return list;
        }

        /// <summary>
        /// Turns active tags into a comma separated string of IDs
        /// </summary>
        /// <param name="Tags">List of Tags to verify which ones are active</param>
        /// <returns></returns>
        public string GetTagIds(List<Tag> Tags)
        {

            string ids = string.Empty;

            if (Tags.Count == 0 || Tags == null)
                return ids;

            if (Tags == null) return ids;

            foreach (var tag in Tags)
            {
                if (tag.isActive)
                    ids += tag.Id.ToString() + ",";
        }

            if (!string.IsNullOrWhiteSpace(ids))
                ids = ids.Substring(0, ids.Length - 1);
            
            return ids;
        }

        /// <summary>
        /// Gets corresponding tags according to the string of ids in the object
        /// </summary>
        /// <param name="source">string of ids in the object</param>
        /// <returns>List of Tags populated with the according tags</returns>
        public async Task<List<Tag>> GetTagsFromString(string source)
        {
            List<Tag> tags = new List<Tag>();

            if (string.IsNullOrEmpty(source))
                return tags;

            string[] ids = source.Split(',');

            if (ids.Length <= 0) return tags;

            foreach (var id in ids)
            {
                tags.Add(await GetByIdAsync(int.Parse(id)));
            }

            return tags;
        }


        public async Task<List<Tag>> MatchTagList(string source)
        {
            var tags = await GetTagsList();
            if (string.IsNullOrEmpty(source))
                return tags;

            string[] ids = source.Split(',');

            foreach (var item in ids)
            {
                foreach (var tag in tags)
                {
                    if (int.Parse(item) == tag.Id)
                        tag.isActive = true;
                }
            }
            return tags;
        }

        public async Task<Tag> GetTagByName(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<List<Book>> GetBooksWithTags(List<Book> source, string tagName)
        {
            List<Book> books = new List<Book>();
            
            if (source.Count == 0)
                return books;

            var tag = await GetTagByName(tagName);
            if (tag == null) return books;

            foreach (var book in source)
            {
                if (!string.IsNullOrEmpty(book.tagIds))
                {
                    string[] separateIds = book.tagIds.Split(',');

                    foreach (var tagId in separateIds)
                    {
                        if (tagId == tag.Id.ToString())
                        {
                            books.Add(book);
                        }
                    }
                }
            }
            
            return books;
        }
    }
}
