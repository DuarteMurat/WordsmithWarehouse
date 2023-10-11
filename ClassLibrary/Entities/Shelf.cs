using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Shelf :IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string BookIds {get; set; }

        public string? Description { get; set; }

        public List<Book> Books { get; set; }
    }
}
