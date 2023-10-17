using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace WordsmithWarehouse.Models
{
    public class LibraryListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        [Display(Name = "Address")]
        public string Adress { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:HH:mm}" + "h")]
        [Display(Name = "Opening Hour")]
        public DateTime OpeningHour { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:HH:mm}" + "h")]
        [Display(Name = "Closing Hour")]
        public DateTime ClosingHour { get; set; }

        public bool IsOpened { get; set; }
    }
}
