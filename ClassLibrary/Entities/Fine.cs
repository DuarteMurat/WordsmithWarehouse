using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Fine: IEntity
    {
        public int Id { get; set; }

        public int LeaseId { get; set; }

        public string UserId { get; set; }

        public float ChargeAmount { get; set; }

        public bool IsPaid { get; set; }
    }
}
