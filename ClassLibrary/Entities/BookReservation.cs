using ClassLibrary.Data;
using System;
using System.Collections.Generic;
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
        
        public DateTime ReservationDate { get; set; } 
        
        public DateTime PickupDate { get; set; } 

        public DateTime ReturnDate { get; set; }
        
        public bool IsCancelled { get; set; }

        public bool IsCompleted { get; set; }

        public string BookIds { get; set; }
    }
}
