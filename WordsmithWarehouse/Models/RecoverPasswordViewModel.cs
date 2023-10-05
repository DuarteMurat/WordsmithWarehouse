using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
