using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class BookComments : IEntity
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string CommentIds { get; set; }
    }
}
