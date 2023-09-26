using ClassLibrary.Entities;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class BookReservationViewModel : BookReservation
    {
        public List<Book> Books { get; set; }
    }
}
