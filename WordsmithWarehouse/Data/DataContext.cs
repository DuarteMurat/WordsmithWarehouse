using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Data
{
    public class DataContext : IdentityDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BookReservation> BookReservations { get; set; }
        public DbSet<BookComments> BookComments { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<BookQuantity> BookQuantity { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Message> Messages { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            base.OnModelCreating(builder);
        }

        public DbSet<ClassLibrary.Entities.Lease> Lease { get; set; }

        public DbSet<WordsmithWarehouse.Models.ManageUserViewModel> ManageUserViewModel { get; set; }
    }
}
