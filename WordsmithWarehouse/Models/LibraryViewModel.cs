using ClassLibrary.Entities;
using Microsoft.CodeAnalysis;
using System;

namespace WordsmithWarehouse.Models
{
    public class LibraryViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Adress { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime OpeningHour { get; set; }

        public DateTime ClosingHour { get; set;}

        public TimeSpan OpenDuration => ClosingHour - OpeningHour;

        public bool IsOpened { get; set; }
    }
}
