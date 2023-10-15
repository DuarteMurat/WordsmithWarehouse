using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Author : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} can only contain up to {1} characters.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display (Name ="Photo")]
        public string ImageURL { get; set; }
    }
}
