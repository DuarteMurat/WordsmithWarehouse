using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    internal class BookQuantity
    {
        public int Id { get; set; }

        public int BookId {  get; set; }

        public int LibraryId { get; set; }

        public int Stock { get; set; }

        public int StockBeingUsed { get; set; }
    }
}
