using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Book
    {

        #region Main Book Information
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string? Subtitle { get; set; }
        
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }
        #endregion

        #region Book Avaialability
        [Display(Name = "Available Online")]
        public bool IsAvailableOnline { get; set; }
        [Display(Name = "Available")]
        public bool IsAvailablePhysical { get; set; }
        #endregion

        #region Book Image
        public string ImageURL { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageURL))
                    return null;

                return $"https://localhost:44309{ImageURL.Substring(1)}";
            }
        }
        #endregion
    }
}
