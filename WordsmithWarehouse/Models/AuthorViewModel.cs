using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WordsmithWarehouse.Models
{
    public class AuthorViewModel
    {
        [Display(Name = "Photo")]
        public IFormFile ImageFile { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Photo")]
        public string ImageURL { get; set; }
    }
}
