using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserImage { get; set; }

        public string UserIdString { get; set; }

        public string Text { get; set; }

        public float Rating { get; set; }

        public int BookId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
        
    }
}
