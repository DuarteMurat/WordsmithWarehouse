using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WordsmithWarehouse.Models
{
    public class AuthorViewModel
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
        
        // added after globalviewmodel

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string ImageURL { get; set; }
    }
}
