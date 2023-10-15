using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibrary.Entities
{
    public class Lease : IEntity
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int LibraryId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Pickup Date")]
        public DateTime? PickUpDate { get; set; }

        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "Lease Time")]
        public DateTime? LeaseTime { get; set; }

        [Display(Name = "On Going")]
        public bool OnGoing { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }
    }
}
