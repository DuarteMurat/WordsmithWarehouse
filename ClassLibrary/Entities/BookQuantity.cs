using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class BookQuantity : IEntity
    {
        public int Id { get; set; }

        public int BookId {  get; set; }

        public int LibraryId { get; set; }

        public int TotalStock { get; set; }

        public int StockAvailable { get; set; }

        public int stockOnHold { get; set; }
        
        public int StockBeingUsed { get; set; }
    }
}
