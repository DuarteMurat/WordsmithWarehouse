using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Library : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Adress { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:HH:mm}" + "h")]
        public DateTime OpeningHour { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:HH:mm}" + "h")]
        public DateTime ClosingHour { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:HH:mm}" + "h")]
        public TimeSpan OpenDuration => ClosingHour - OpeningHour;

        public bool IsOpened { get; set; }
    }
}
