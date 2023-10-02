﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WordsmithWarehouse.Models
{
    public class ManageUserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string ImageURL { get; set; }
    }
}