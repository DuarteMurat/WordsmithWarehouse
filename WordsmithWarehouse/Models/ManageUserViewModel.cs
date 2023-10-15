using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WordsmithWarehouse.Models
{
    public class ManageUserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
        [Display(Name = "Profile Picture")]
        public string ImageURL { get; set; }
    }
}
