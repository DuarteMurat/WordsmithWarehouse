using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class LoginViewModel : GlobalViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
