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

        Task CreateBookTags(Book book, List<Tag> listOfTags);

        Tag GetTagByName(string name);

        Task<List<BookTags>> GetActiveBookTags(Book book);

        List<Tag> GetActiveTags(BookViewModel model);


    }
}