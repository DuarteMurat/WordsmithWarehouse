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

        public int BookId { get; set; }

        public int LibraryId { get; set; }

        public string UserId { get; set; }

        public int QueueNumber { get; set; }

        public DateTime ReservationDate { get; set; }
    }
}
