using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsmithWarehouse.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {

        List<Tag> GetTagsList();

        Task CreateBookTags(Book book, List<Tag> listOfTags);
    }
}