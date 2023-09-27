using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class BookReservation : IEntity
    {
        public int Id { get; set; }
        
        public User User { get; set; } 

        public Library Library { get; set; }

        [Display(Name = "Reservation Date")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Pickup Date")]
        public DateTime PickupDate { get; set; }

        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Reservation Cancelled")]
        public bool IsCancelled { get; set; }

        [Display(Name = "Reservation Completed")]
        public bool IsCompleted { get; set; }

        public string BookIds { get; set; }
    }
}
