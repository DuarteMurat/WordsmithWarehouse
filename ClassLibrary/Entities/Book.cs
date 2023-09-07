using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibrary.Entities
{
    public class Book : IEntity
    {
        #region Main Book Information
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} can only contain up to {1} character")]
        public string Title { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} can only contain up to {1} character")]
        public string? Subtitle { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "The field {0} can only contain up to {1} character")]
        public string ISBN { get; set; }

        [Required]
        public Author Author { get; set; }

        public int AuthorId { get; set; }

        public User User { get; set; }

        public ICollection<BookAuthors> BookAuthors { get; set; }
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
