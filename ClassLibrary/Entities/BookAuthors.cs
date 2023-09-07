using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class BookAuthors
    {
        public int Id { get; set; }

        public Book Book { get; set; }

        public int BookId { get; set; }

        public Author Author { get; set; }

        public int AuthorId { get; set; }
    }
}
