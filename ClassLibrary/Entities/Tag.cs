using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Tag : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool isActive { get; set; }

        [Display(Name = "Administrative Tag")]
        public bool isAdmin { get; set; }
    }
}
