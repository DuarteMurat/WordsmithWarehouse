using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entities
{
    public class Penalty : IEntity
    {
        public int Id { get; set; }

        public int LeaseID { get; set; }

        public DateTime EffectiveReturnDate { get; set; }

        public DateTime DelayedReturnDate { get; set; }

        public float PenaltyValue {get; set; }
    }
}
