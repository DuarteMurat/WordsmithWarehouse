using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WordsmithWarehouse.Models
{
    public class AuthorViewModel : Author
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
