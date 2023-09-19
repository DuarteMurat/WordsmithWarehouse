using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        /// <summary>
        /// Populates a list with all tags present in the database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Turns active tags into a comma separated string of IDs
        /// </summary>
        /// <param name="Tags">List of Tags to verify which ones are active</param>
        /// <returns></returns>
        public string GetTagIds(List<Tag> Tags)
        {

            string ids = string.Empty;

            if (Tags == null) return ids;

            foreach (var tag in Tags)
            {
                if (tag.isActive)
                    ids += tag.Id.ToString() + ",";
            }

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
            string[] ids = source.Split(',');
            List<Tag> tags = new List<Tag>();

            if (ids.Length <= 0) return tags;

            foreach (var id in ids)
            {
                tags.Add(await GetByIdAsync(int.Parse(id)));
            }

            return tags;
        }


        public List<Tag> MatchTagList(string source)
        {
            var tags = GetTagsList();
            string[] ids = source.Split(',');

            if (ids.Length <= 0) return tags;

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
    }
}
