using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class SearchBookViewModel
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string ImageURL { get; set; }

        public bool IsAvailableOnline { get; set; }

        public bool IsAvailablePhysical { get; set; }

        public int AuthorId { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Tag> ActiveTags { get; set; }

        public Author Author { get; set; }
    }
}
