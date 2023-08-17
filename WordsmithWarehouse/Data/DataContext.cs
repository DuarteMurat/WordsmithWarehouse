using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WordsmithWarehouse.Data
{
    public class DataContext : IdentityDbContext
    {
        public DbSet<Book> Books { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
