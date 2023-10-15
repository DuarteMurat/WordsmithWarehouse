using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
        public string Username { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        public string ImageURL { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ImageFile { get; set; }
    }
}
