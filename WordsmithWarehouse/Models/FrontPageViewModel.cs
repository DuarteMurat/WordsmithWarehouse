using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WordsmithWarehouse.Models
{
    public class FrontPageViewModel
    {
        public List<Book> Books { get; set; }

        public string BestSellerBooks { get; set; }

        public string ImageURL { get; set; }
    }
}
