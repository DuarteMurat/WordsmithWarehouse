using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class BookReservationViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int LibraryId { get; set; }

        public int BookId { get; set; }

        public DateTime ReservationDate { get; set; }

    }
}
