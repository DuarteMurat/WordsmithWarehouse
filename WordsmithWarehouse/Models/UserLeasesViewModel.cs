using ClassLibrary.Entities;
using System.Collections.Generic;

namespace WordsmithWarehouse.Models
{
    public class UserLeasesViewModel
    {
        public User User { get; set; }

        public List<Lease> Leases{ get; set; }

        public List<Book> Books { get; set; }

        public List<Library> Libraries { get; set; }

        public Lease ExampleLease { get; set; }
        
    }
}
