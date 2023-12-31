﻿using ClassLibrary.Data;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(13, ErrorMessage = "The field {0} can only contain up to {1} character")]
        public string ISBN { get; set; }

        public Author? Author { get; set; }

        [Required]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public int TotalStock { get; set; }

        public string tagIds { get; set; }

        public User User { get; set; }

        public string CoverType { get; set; }

        public string Pages { get; set; }

        public string Publisher { get; set; }

        public string Synopsis { get; set; }

        public int ReleaseYear { get; set; }

        public string BookFile { get; set; }

        public string BookFileURL { get; set; }
        #endregion

        #region Book Avaialability
        [Display(Name = "Available Online")]
        public bool IsAvailableOnline { get; set; }
        [Display(Name = "Available in Store")]
        public bool IsAvailablePhysical { get; set; }
        #endregion

        #region Book Image
        [Display(Name = "Image")]
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
