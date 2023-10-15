using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class ShelfViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string BookIds { get; set; }

        public string Description { get; set; }
    }
}
