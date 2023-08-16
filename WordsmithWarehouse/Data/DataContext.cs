using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace WordsmithWarehouse.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}
