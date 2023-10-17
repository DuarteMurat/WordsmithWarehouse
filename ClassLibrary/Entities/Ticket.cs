using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Username { get; set; }

        public string UserEmail { get; set; }

        public bool Open { get; set; }

        public bool Close { get; set; }
    }
}
