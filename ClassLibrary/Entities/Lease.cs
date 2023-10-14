using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Lease : IEntity
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int LibraryId { get; set; }

        public string UserId { get; set; }

        public DateTime? PickUpDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime? LeaseTime { get; set; }

        public bool OnGoing { get; set; }

        public bool IsCompleted { get; set; }
    }
}
