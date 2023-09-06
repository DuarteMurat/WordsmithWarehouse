using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }
}
