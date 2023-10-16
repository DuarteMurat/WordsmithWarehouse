using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class BookViewModel
    {
        [Display(Name = "Cover")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }

        [Display(Name = "Tags")]
        public List<Tag> Tags { get; set; }

        public Author ModelAuthor { get; set; }

        // added after globalviewmodel

        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        [StringLength(13)]
        public string ISBN { get; set; }

        public Author Author { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public string tagIds { get; set; }

        public User User { get; set; }

        [Display(Name = "Cover type")]
        public string CoverType { get; set; }

        public string Pages { get; set; }

        public string Publisher { get; set; }

        public string Synopsis { get; set; }

        [Display(Name = "Online")]
        public bool IsAvailableOnline { get; set; }

        [Display(Name = "In Storage")]
        public bool IsAvailablePhysical { get; set; }

        [Display(Name = "Book Cover")]
        public string ImageURL { get; set; }

        [Display(Name = "Release Year")]
        [MaxLength(4)]
        public int ReleaseYear { get; set; }

        [Display(Name = "Book File")]
        public IFormFile BookFile { get; set; }

        public string BookFileUrl { get; set; }
    }
}