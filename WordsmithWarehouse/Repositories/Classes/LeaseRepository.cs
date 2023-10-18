using ClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Repositories.Classes
{
    public class LeaseRepository : GenericRepository<Lease>, ILeaseRepository
    {
        private readonly DataContext _context;

        public LeaseRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Fine>> GetFinesAsync()
        {
            var leaseList = await GetAll().ToListAsync();
            List<Fine> fines = new List<Fine>();

            var CurrentDay = DateTime.Now;

            foreach (var item in leaseList)
            {
                if (CurrentDay > item.ReturnDate)
                {
                    var dateDifference = CurrentDay - item.ReturnDate;

                    Fine fine = new Fine
                    {
                        LeaseId = item.Id,
                        UserId = item.UserId,
                        IsPaid = false,
                        ChargeAmount = dateDifference.Value.Days * 2.5f, 
                    };

                    fines.Add(fine);
                }
            }

            return fines;
        }
    }
}
