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

        //[Display(Name = "Tags")]
        //[Range(1, int.MaxValue, ErrorMessage ="Please Select a Tag")]

        //public int TagId { get; set; }

        //public IEnumerable<SelectListItem> Tags { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }


    }
}
