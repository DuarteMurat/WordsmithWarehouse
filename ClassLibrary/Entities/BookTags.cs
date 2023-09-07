using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class BookTags : IEntity
    {
        public int Id { get; set; }

        public Book Book { get; set; }

        public int BookId { get; set; }

        public Tag Tag { get; set; }

        public int TagId { get; set; }
    }
}
