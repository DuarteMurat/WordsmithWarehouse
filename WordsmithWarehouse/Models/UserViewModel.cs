﻿using ClassLibrary.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WordsmithWarehouse.Models
{
    public class UserViewModel : User
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public string Password { get; set; }
    }
}
