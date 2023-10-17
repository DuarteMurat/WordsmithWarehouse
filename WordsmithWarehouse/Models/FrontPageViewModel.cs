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

        public List<Book> BestSellerBooks { get; set; }

        public List<Book> BookOfTheMonth { get; set; }

        public List<Book> NewReleases { get; set; }

        public List<Book> Classics { get; set; }

        public string ImageURL { get; set; }
    }
}
