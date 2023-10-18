using ClassLibrary.Entities;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class BookStockViewModel
    {
        public int Id { get; set; }

        public List<Library> Libraries { get; set; }

        public Book Book { get; set; }


    }
}
