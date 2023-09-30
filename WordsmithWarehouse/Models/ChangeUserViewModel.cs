using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WordsmithWarehouse.Models
{
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string ImageURL { get; set; }
    }
}
