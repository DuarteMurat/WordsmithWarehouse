using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class LeaseViewModel
    {
        public int Id { get; set; }

        public Book Book { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public User User { get; set; }

        public int LibraryId { get; set; }

        public Library Library { get; set; }

        public IEnumerable<SelectListItem> LibraryList { get; set; }

        public List<Library> Libraries { get; set; }

        public DateTime? PickUpDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime? LeaseTime { get; set; }

        public bool OnGoing { get; set; }

        public bool IsCompleted { get; set; }

        public string ErrorMessage { get; set; }

        public bool ViewReturn { get; set; }
    }
}
