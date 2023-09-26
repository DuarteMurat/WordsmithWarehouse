using ClassLibrary.Entities;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class FrontPageViewModel
    {
        public List<Book> Books { get; set; }

        public string BestSellerBooks { get; set; }
    }
}
