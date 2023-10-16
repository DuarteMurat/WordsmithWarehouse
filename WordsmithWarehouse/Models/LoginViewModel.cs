using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }

        public string Twofa { get; set; }

        public bool IsTwofa { get; set; }
    }
}
