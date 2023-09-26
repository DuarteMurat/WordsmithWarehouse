using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {

        List<Tag> GetTagsList();

        string GetTagIds(List<Tag> Tags);

        Task<List<Tag>> GetTagsFromString(string source);

        List<Tag> MatchTagList(string source);

        Task<string> GetBooksWithTags(List<Book> source, string tagName);
    }
}