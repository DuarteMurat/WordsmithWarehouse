using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class BookViewModel : Book
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }

        public List<Tag> Tags { get; set; }

        public Author ModelAuthor { get; set; }
    }
}