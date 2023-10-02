using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class BookReservationViewModel : GlobalViewModel
    {
        public List<Book> Books { get; set; }

        // added after globalviewmodel

        public int Id { get; set; }

        public User User { get; set; }

        public Library Library { get; set; }

        public DateTime ReservationDate { get; set; }

        public DateTime ReturnDate {  get; set; }

        public DateTime PickupDate { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsCompleted { get; set; }

        public string BookIds { get; set; }

    }
}
